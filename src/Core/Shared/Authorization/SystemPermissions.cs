using System.Collections.ObjectModel;

namespace Microsoft.Teams.Assist.Shared.Authorization;

public static class SystemAction
{
    public const string View = nameof(View);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    //public const string ManageRoles = nameof(ManageRoles);
    //public const string ManagePermissions = nameof(ManagePermissions);

}

public static class SystemResource
{
    public const string Users = nameof(Users);
    public const string Roles = nameof(Roles);
    public const string ManageLookUps = nameof(ManageLookUps);
    public const string ManageSettings = nameof(ManageSettings);
    public const string EmailLog = nameof(EmailLog);
}

public class SystemPermissions
{
    private static readonly SystemPermission[] _all = new SystemPermission[]
    {
        // Manage Roles
        new("View Roles", SystemAction.View, SystemResource.Roles),
        new("Create Roles", SystemAction.Create, SystemResource.Roles),
        new("Update Roles", SystemAction.Update, SystemResource.Roles),
        new("Delete Role", SystemAction.Delete, SystemResource.Roles),

        // Manage Users
        new("View User", SystemAction.View, SystemResource.Users),
        new("Create User", SystemAction.Create, SystemResource.Users),
        new("Update User", SystemAction.Update, SystemResource.Users),

        // Manage Lookup and LookupValues
        new("View Lookups and Values", SystemAction.View, SystemResource.ManageLookUps),
        new("Create Lookup Values", SystemAction.Create, SystemResource.ManageLookUps),
        new("Update Lookup Values", SystemAction.Update, SystemResource.ManageLookUps),

        // Manage Settings
        new("View Forms", SystemAction.View, SystemResource.ManageSettings),
        new("Update Forms", SystemAction.Update, SystemResource.ManageSettings),

        // Email Log
        new("View Email Log", SystemAction.View, SystemResource.EmailLog),
    };

    public static IReadOnlyList<SystemPermission> All { get; } = new ReadOnlyCollection<SystemPermission>(_all);
    public static IReadOnlyList<SystemPermission> Root { get; } = new ReadOnlyCollection<SystemPermission>(_all.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<SystemPermission> Admin { get; } = new ReadOnlyCollection<SystemPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<SystemPermission> Basic { get; } = new ReadOnlyCollection<SystemPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record SystemPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
