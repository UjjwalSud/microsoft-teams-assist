using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Teams.Assist.Application.Common.Exceptions;
using Microsoft.Teams.Assist.Application.Common.Extensions;
using Microsoft.Teams.Assist.Application.Common.Interfaces;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Users;
using Microsoft.Teams.Assist.Application.Setting.Models;
using Microsoft.Teams.Assist.Domain.Enums;
using Microsoft.Teams.Assist.Infrastructure.Auth;
using Microsoft.Teams.Assist.Infrastructure.Nexus.Identity.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Nexus.MultiTenant.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Nexus;
using Microsoft.Teams.Assist.Shared.Authorization;
using Microsoft.Teams.Assist.Shared.Nexus;

namespace Microsoft.Teams.Assist.Infrastructure.Persistence.Initialization;
internal class ApplicationDbSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ApplicationDbSeeder> _logger;
    private readonly ISerializerService _serializer;
    private readonly NexusDbContext _nexusDbContext;
    private readonly IUserService _userService;
    private readonly ICurrentUserInitializer _currentUserInitializer;

    public ApplicationDbSeeder(RoleManager<ApplicationRole> roleManager, NexusDbContext nexusDbContext, UserManager<ApplicationUser> userManager, ILogger<ApplicationDbSeeder> logger, ISerializerService serializer, IUserService userService, ICurrentUserInitializer currentUserInitializer)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
        _serializer = serializer;
        _nexusDbContext = nexusDbContext;
        _userService = userService;
        _currentUserInitializer = currentUserInitializer;
    }

    public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, NexusDbContext nexusDbContext, Tenants currentTenant, CancellationToken cancellationToken)
    {
        //_currentUserInitializer.SetCurrentUserId(userId);
        _currentUserInitializer.SetCurrentTenant(currentTenant.Id, currentTenant.UniqueId);

        await _userService.AssignDefaultRoleToNewTenantAsync(currentTenant.Id, currentTenant.UniqueId, cancellationToken);

        await SeedAdminUserAsync(nexusDbContext, currentTenant);
        //await _seederRunner.RunSeedersAsync(cancellationToken);
        await SeedLookUps(dbContext, currentTenant, cancellationToken);
        await SeedSettings(dbContext, currentTenant, cancellationToken);
    }

    private async Task SeedAdminUserAsync(NexusDbContext dbContext, Tenants tenant)
    {
        if (tenant.UniqueId != NexusConstants.Root.TenantUniqueId)
        {
            return;
        }

        if (await _userManager.Users.FirstOrDefaultAsync(u => u.Id == NexusConstants.Root.UserId)
            is not ApplicationUser adminUser)
        {
            string adminUserName = $"{tenant.Name.Trim()}.{SystemRoles.Admin}".ToLowerInvariant();
            adminUser = new ApplicationUser
            {
                Id = NexusConstants.Root.UserId,
                FirstName = tenant.Name.Trim().ToLowerInvariant(),
                LastName = SystemRoles.Admin,
                Email = NexusConstants.Root.EmailAddress,
                UserName = adminUserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = NexusConstants.Root.EmailAddress.ToUpperInvariant(),
                NormalizedUserName = adminUserName.ToUpperInvariant(),
                IsActive = true,
                FKTenantId = tenant.Id,
            };

            _logger.LogInformation("Seeding Default Admin User for '{tenantId}' Tenant.", tenant.Name);
            var password = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = password.HashPassword(adminUser, NexusConstants.Root.DefaultPassword);
            await _userManager.CreateAsync(adminUser);

        }

        // Assign role to user
        if (!await _userManager.IsInRoleAsync(adminUser, SystemRoles.FormatTenantRoleName(SystemRoles.Admin, tenant.Id)))
        {
            _logger.LogInformation("Assigning Admin Role to Admin User for '{tenantId}' Tenant.", tenant.Id);
            await _userManager.AddToRoleAsync(adminUser, SystemRoles.FormatTenantRoleName(SystemRoles.Admin, tenant.Id));
        }
    }

    private async Task SeedLookUps(ApplicationDbContext dbContext, Tenants tenant, CancellationToken cancellationToken)
    {
        foreach (LookUpCodeTypes type in Enum.GetValues(typeof(LookUpCodeTypes)))
        {
            if (!await dbContext.LookUpCodes.AnyAsync(x => x.LookUpCodeType == type))
            {
                _logger.LogInformation($"{type.GetDescription()} added for tenant {tenant.Name}");
                await dbContext.LookUpCodes.AddAsync(new Domain.LookUp.LookUpCodes
                {
                    LookUpCodeType = type,
                    Description = type.GetDescription()
                });
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedSettings(ApplicationDbContext dbContext, Tenants tenant, CancellationToken cancellationToken)
    {
        foreach (SettingTypes type in Enum.GetValues(typeof(SettingTypes)))
        {
            if (!await dbContext.Settings.AnyAsync(x => x.SettingType == type))
            {
                _logger.LogInformation($"{type.GetDescription()} added for tenant {tenant.Name}");

                switch (type)
                {
                    //default:
                        //throw new CustomNotImplementedException($"{type.ToString()} not setup");
                }
            }
        }

        await dbContext.SaveChangesAsync();
    }
}