using Mapster;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Teams.Assist.Application.Common.Exceptions;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Request;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Response;
using Microsoft.Teams.Assist.Application.Nexus.MultiTenant.Models;
using Microsoft.Teams.Assist.Application.Nexus.MultiTenant.Models.Request;
using Microsoft.Teams.Assist.Application.Nexus.Subscription.Models;
using Microsoft.Teams.Assist.Domain.Enums;
using Microsoft.Teams.Assist.Infrastructure.Nexus.Identity.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Nexus.Identity.Extensions;
using Microsoft.Teams.Assist.Infrastructure.SystemConstants;
using Microsoft.Teams.Assist.Shared.Authorization;
using System.Text;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Identity;
internal partial class UserService
{
    public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
            IsActive = true,
            FKTenantId = await _subscriptionService.GetRegisteredUserDefaultSubscriptionAsync(),
        };
        var validateUser = await ValidateUserAndPasswordAsync(user, request.Password);
        if (!validateUser.Succeeded)
        {
            throw new BadRequestException(ErrorMessages.IdentityValidationError, validateUser.GetErrors());
        }

        var createTenantResponse = await _tenantService.CreateAsync(
            new CreateTenantRequest
            {
                UniqueId = Guid.NewGuid(),
                Name = request.FirstName + " " + request.LastName,
                AdminEmail = request.Email,
                IsActive = true,
                FKSubscriptionPKId = -1
            }, cancellationToken);

        user.FKTenantId = createTenantResponse.TenantId;

        var result = await _userManager.CreateAsync(user, request.Password);

        await AssignDefaultRoleToNewTenantAsync(createTenantResponse.TenantId, createTenantResponse.UniqueId, cancellationToken);

        await _userManager.AddToRoleAsync(user, SystemRoles.FormatTenantRoleName(SystemRoles.Admin, createTenantResponse.TenantId));

        if (_securitySettings.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _jobService.Enqueue(() => _mailService.EmailRegistrationVerificationEmailAsync(user.Id, code, CancellationToken.None));
        }

        await _databaseInitializer.InitializeApplicationDbForTenantAsync(await _nexusDbContext.Tenants.SingleAsync(x => x.Id == createTenantResponse.TenantId), cancellationToken);

        return new RegisterUserResponse { UserId = user.Id, Message = string.Format(SuccessMessages.UserRegistered, user.UserName, user.Email) };
    }

    public async Task<CreateUserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        int tenantId = _currentUser.GetTenant();

        int totalUsers = await GetCountAsync(cancellationToken);
        TenantsDto tenantsDto = await _tenantService.GetByIdAsync(tenantId, cancellationToken);
        SubscriptionRulesDto subscriptionRules = await _subscriptionService.SubscriptionRulesByIdAsync(tenantsDto.FKSubscriptionPKId);

        if (totalUsers == subscriptionRules.MaxUsers)
        {
            throw new BadRequestException(ErrorMessages.SubscriptionUserCannotAdd);
        }


        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
            IsActive = true,
            FKTenantId = tenantId
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new BadRequestException(ErrorMessages.IdentityValidationError, result.GetErrors());
        }

        await _userManager.AddToRoleAsync(user, SystemRoles.FormatTenantRoleName(SystemRoles.Basic, user.FKTenantId));

        return new CreateUserResponse { Message = string.Format(SuccessMessages.UserCreated, user.UserName), UserId = user.Id };
    }

    public async Task<List<ViewUserDetailsResponse>> GetListAsync(CancellationToken cancellationToken)
    {
        var userlist = new List<ViewUserDetailsResponse>();

        var users = await _userManager.Users
               .AsNoTracking()
               .Where(x => x.FKTenantId == _currentUser.GetTenant())
               .ToListAsync();

        foreach (var item in users)
        {
            userlist.Add(new ViewUserDetailsResponse
            {
                Id = new Guid(item.Id),
                UserName = item.UserName,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                IsActive = item.IsActive,
                EmailConfirmed = item.EmailConfirmed,
                PhoneNumber = item.PhoneNumber,
                ImageUrl = item.ImageUrl,
            });
        }

        return userlist.Adapt<List<ViewUserDetailsResponse>>();
    }

    public async Task<ViewUserDetailsResponse> GetAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Where(u => u.Id == userId && u.FKTenantId == _currentUser.GetTenant())
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "User"));

        var userProfileDetails = user.Adapt<ViewUserDetailsResponse>();
        if (!string.IsNullOrEmpty(userProfileDetails.ImageUrl))
        {
            userProfileDetails.ImageUrl = _fileStorage.FileToBase64String(userProfileDetails.ImageUrl);
        }

        return userProfileDetails;
    }

    public async Task<ViewUserDetailsDto> GetByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Where(u => u.Id == userId && u.FKTenantId == _currentUser.GetTenant())
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "User"));

        return user.Adapt<ViewUserDetailsDto>();
    }

    public async Task<string> UpdateUserAsync(UpdateUserDetailsRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "User"));

        //bool isAdmin = await _userManager.IsInRoleAsync(user, SystemRoles.Admin);
        //if (isAdmin)
        //{
        //    throw new ConflictException(_t["Administrators Profile's Status cannot be toggled"]);
        //}
        if (request.UserId != _currentUser.GetUserId().ToString())
        {
            user.IsActive = request.IsActive;
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        //user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;
        //user.EmailConfirmed = request.EmailConfirmed;

        await _userManager.UpdateAsync(user);

        return SuccessMessages.UpdateUser;
    }

    public async Task<string> UpdateAsync(UpdateUserRequest request, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "User"));

        string currentImage = user.ImageUrl ?? string.Empty;
        if (request.Image != null || request.DeleteCurrentImage)
        {
            user.ImageUrl = await _fileStorage.UploadAsync<ApplicationUser>(request.Image, FileType.Image);
            if (request.DeleteCurrentImage && !string.IsNullOrEmpty(currentImage))
            {
                //string root = Directory.GetCurrentDirectory();
                //_fileStorage.Remove(Path.Combine(root, currentImage));
                _fileStorage.Remove(currentImage);
            }
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        string? phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (request.PhoneNumber != phoneNumber)
        {
            await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
        }

        var result = await _userManager.UpdateAsync(user);

        await _signInManager.RefreshSignInAsync(user);

        if (!result.Succeeded)
        {
            throw new BadRequestException(ErrorMessages.UpdateProfileFailed, result.GetErrors());
        }

        return SuccessMessages.UpdateProfile;
    }
}
