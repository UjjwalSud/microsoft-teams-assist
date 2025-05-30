namespace Microsoft.Teams.Assist.Infrastructure.SystemConstants;
internal class ErrorMessages
{
    public readonly static string AlreadyExists = "{0} already exists";
    public readonly static string ItemNotFound = "{0} not found";
    public readonly static string NotImplementedItem = "{0} not implemented";
    public static readonly string IdentityValidationError = "Validation Errors Occurred.";

    public static readonly string RootTenantRoleCannotBeRemoved = "Cannot Remove Admin Role From Root Tenant Admin.";
    public static readonly string MinimumAdminMessage = "Tenant should have at least 2 Admins.";
    public static readonly string CreateRoleFailed = "Register role failed";
    public static readonly string UpdateRoleFailed = "Register role failed";
    public static readonly string NotAllowedToDeleteRole = "Not allowed to delete {0} Role as it is being used.";
    public static readonly string UpdateProfileFailed = "Update profile failed";
    public static readonly string ChangePasswordFailed = "Change password failed";
    public static readonly string SubscriptionUserCannotAdd = $"Please upgrade your subscription to add more users";
    public static readonly string ConfirmEmail = "An error occurred while confirming E-Mail.";
    public static readonly string GenericError = "An Error has occurred!";
    public static readonly string AuthenticationFailed = "Authentication Failed";
    public static readonly string AuthenticationUserNotActive = "User Not Active. Please contact the administrator.";
    public static readonly string AuthenticationEmailNotConfirmed = "E-Mail not confirmed.";
    public static readonly string AuthenticationInvalidRefreshToken = "Invalid Refresh Token.";
    public static readonly string AuthenticationInvalidToken = "Invalid Token.";
    public static readonly string AuthenticationTenantNotActive = "Tenant is not Active. Please contact the Application Administrator.";
    public static readonly string UpdatePermissionsFailed = "Update permissions failed.";
    public static readonly string UpdateCantModifyPermission = "Not allowed to modify Permissions for this Role.";
    public static readonly string CantDeleteRole = "Not allowed to delete {0} Role.";
    public static readonly string NotAuthorized = "You are not authorized to access this resource.";
    public static readonly string ItemAlreadyExists = "Item with the name '{0} already exists.";
    public static readonly string OptionsRequired = "Options are required for this field type.";
    public static readonly string HeadersRequired = "Headers are required for this field type.";
    public static readonly string UpdateSettingsDeserializeCrash = "Invalid setting details";


}

internal class SuccessMessages
{
    public static readonly string UserRegistered = "User {0} Registered. Please check {1} to verify your account!";
    public static readonly string UserCreated = "User {0} Created.";
    public static readonly string RoleCreated = "Role {0} Created.";
    public static readonly string RoleUpdated = "Role {0} Updated.";
    public static readonly string RoleDeleted = "Role {0} Deleted.";
    public static readonly string PasswordChanged = "Password Changed Successfully";
    public static readonly string UserRoleAssigned = "User Roles Updated Successfully.";
    public static readonly string ConfirmEmail = "Account Confirmed for E-Mail {0}. Please login";
    public static readonly string ForgotPassword = "Password Reset Mail has been sent to your authorized Email.";
    public static readonly string ForgotPasswordReset = "Password changed successfully, please login";
    public static readonly string UpdatePermissions = "Role permissions updated successfully.";
    public static readonly string UpdateUser = "User updated successfully.";
    public static readonly string UpdateProfile = "Profile updated successfully.";
    public static readonly string RecordAddedSuccessfully = "Record added successfully.";
    public static readonly string RecordUpdatedSuccessfully = "Record updated successfully.";

}

internal class LoggerMessages
{
}
