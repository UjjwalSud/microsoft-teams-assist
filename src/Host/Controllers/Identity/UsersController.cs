using Microsoft.Teams.Assist.Application.Common.Exceptions;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Users;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Request;
using Microsoft.Teams.Assist.Application.Nexus.Identity.Users.Models.Response;
using Microsoft.Teams.Assist.Host.Controllers.BaseControllers;

namespace Microsoft.Teams.Assist.Host.Controllers.Identity;

public class UsersController : VersionNeutralApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    [HttpPost("register")]
    [AllowAnonymous]
    [OpenApiOperation("Anonymous user creates a user.", "")]
    public Task<RegisterUserResponse> SelfRegisterAsync(RegisterUserRequest request)
    {
        return _userService.RegisterUserAsync(request);
    }

    [HttpPost]
    [OpenApiOperation("Creates a new user.", "")]
    [MustHavePermission(SystemAction.Create, SystemResource.Users)]
    public Task<CreateUserResponse> CreateAsync(CreateUserRequest request)
    {
        return _userService.CreateAsync(request);
    }

    [HttpGet]
    [MustHavePermission(SystemAction.View, SystemResource.Users)]
    [OpenApiOperation("Get list of all users.", "")]
    public Task<List<ViewUserDetailsResponse>> GetListAsync(CancellationToken cancellationToken)
    {
        return _userService.GetListAsync(cancellationToken);
    }

    [HttpGet("{id}")]
    [MustHavePermission(SystemAction.View, SystemResource.Users)]
    [OpenApiOperation("Get a user's details.", "")]
    public Task<ViewUserDetailsResponse> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _userService.GetAsync(id, cancellationToken);
    }

    [HttpGet("{id}/roles")]
    [MustHavePermission(SystemAction.View, SystemResource.Users)]
    [OpenApiOperation("Get a user's roles.", "")]
    public Task<List<UserRoleResponse>> GetRolesAsync(string id, CancellationToken cancellationToken)
    {
        return _userService.GetRolesAsync(id, cancellationToken);
    }

    [HttpPost("{id}/roles")]
    [RequireAnyAction([SystemAction.Create, SystemAction.Update], SystemResource.Users)]
    [OpenApiOperation("Update a user's assigned roles.", "")]
    public Task<string> AssignRolesAsync(string id, UserRolesRequest request, CancellationToken cancellationToken)
    {
        return _userService.AssignRolesAsync(id, request, cancellationToken);
    }

    [HttpPost("{id}/update")]
    [MustHavePermission(SystemAction.Update, SystemResource.Users)]
    [OpenApiOperation("Update a user details.", "")]
    public async Task<string> UpdateUserAsync(string id, UpdateUserDetailsRequest request, CancellationToken cancellationToken)
    {
        //if (id != request.UserId)
        //{
        //    throw new BadRequestException("An error has occurred");
        //}
        request.UserId = id;
        return await _userService.UpdateUserAsync(request, cancellationToken);
    }

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    [OpenApiOperation("Confirm email address for a user.", "")]
    public Task<string> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code, CancellationToken cancellationToken)
    {
        return _userService.ConfirmEmailAsync(userId, code, cancellationToken);
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [OpenApiOperation("Request a password reset email for a user.", "")]
    public Task<string> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        return _userService.ForgotPasswordAsync(request);
    }

    [HttpPost("reset-forgot-password")]
    [AllowAnonymous]
    [OpenApiOperation("Reset a user's password.", "")]
    public Task<string> ResetPasswordAsync(ResetForgotPasswordRequest request)
    {
        return _userService.ResetPasswordAsync(request);
    }
}
