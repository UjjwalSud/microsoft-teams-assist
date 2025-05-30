using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Teams.Assist.Application.Common.Caching;
using Microsoft.Teams.Assist.Application.Common.FileStorage;
using Microsoft.Teams.Assist.Application.Common.Interfaces;
using Microsoft.Teams.Assist.Application.Common.Mailing;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Users;
using Microsoft.Teams.Assist.Application.Nexus.MultiTenant;
using Microsoft.Teams.Assist.Application.Nexus.Subscription;
using Microsoft.Teams.Assist.Infrastructure.Auth;
using Microsoft.Teams.Assist.Infrastructure.Nexus.Identity.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Nexus;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Initialization;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Identity;
internal partial class UserService : IUserService
{
    private readonly ICacheService _cache;
    private readonly ICacheKeyService _cacheKeys;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SecuritySettings _securitySettings;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly NexusDbContext _nexusDbContext;
    private readonly ICurrentUser _currentUser;
    private readonly ITenantService _tenantService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IMailService _mailService;
    private readonly IJobService _jobService;
    private readonly IFileStorageService _fileStorage;
    private readonly ICurrentUserInitializer _currentUserInitializer;
    private readonly IDatabaseInitializer _databaseInitializer;

    public UserService(
       SignInManager<ApplicationUser> signInManager,
       ICacheService cache,
       ICacheKeyService cacheKeys,
       UserManager<ApplicationUser> userManager,
       RoleManager<ApplicationRole> roleManager,
       NexusDbContext nexusDbContext,
       ITenantService tenantService,
       ISubscriptionService subscriptionService,
       ICurrentUser currentUser,
       IOptions<SecuritySettings> securitySettings,
       IMailService mailService,
       IJobService jobService,
       IFileStorageService fileStorage,
       ICurrentUserInitializer currentUserInitializer,
       IDatabaseInitializer databaseInitializer)
    {
        _signInManager = signInManager;
        _cache = cache;
        _cacheKeys = cacheKeys;
        _userManager = userManager;
        _roleManager = roleManager;
        _nexusDbContext = nexusDbContext;
        _tenantService = tenantService;
        _subscriptionService = subscriptionService;
        _currentUser = currentUser;
        _securitySettings = securitySettings.Value;
        _mailService = mailService;
        _jobService = jobService;
        _fileStorage = fileStorage;
        _currentUserInitializer = currentUserInitializer;
        _databaseInitializer = databaseInitializer;
    }

    public async Task<IdentityResult> ValidateUserAndPasswordAsync(ApplicationUser user, string password)
    {
        var userValidationResult = await _userManager.UserValidators
            .First()
            .ValidateAsync(_userManager, user);

        if (!userValidationResult.Succeeded)
        {
            return userValidationResult; // Return user validation errors
        }

        var passwordValidationResult = await _userManager.PasswordValidators
            .First()
            .ValidateAsync(_userManager, user, password);

        if (!passwordValidationResult.Succeeded)
        {
            return passwordValidationResult; // Return password validation errors
        }

        return IdentityResult.Success; // Both validations succeeded
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken) =>
       _userManager.Users.AsNoTracking().CountAsync(x => x.FKTenantId == _currentUser.GetTenant(), cancellationToken);
}
