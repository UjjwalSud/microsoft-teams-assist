using System.Collections.ObjectModel;

namespace Microsoft.Teams.Assist.Shared.Authorization;
public class SystemRoles
{
    public const string Admin = nameof(Admin);
    public const string Basic = nameof(Basic);

    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
        Admin,
        Basic
    });

    public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
    public static bool IsDefaultForTenant(string roleName)
    {
        var s = roleName.Split(TenantRoleNameSplitter)[1];
        return IsDefault(s);
    }

    public static string GetRoleNameWithoutTenantName(string roleName)
    {
        return roleName.Split(TenantRoleNameSplitter)[1];
    }

    public static string FormatTenantRoleName(string roleName, int tenantId)
    {
        return $"{tenantId}{TenantRoleNameSplitter}{roleName}";
    }

    public static readonly char TenantRoleNameSplitter = '_';
}
