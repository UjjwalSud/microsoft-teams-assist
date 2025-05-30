using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Teams.Assist.Domain.Enums;
using Microsoft.Teams.Assist.Application.Common.Extensions;

namespace Microsoft.Teams.Assist.Infrastructure.FileStorage;

internal static class Startup
{
    internal static IApplicationBuilder UseFileStorage(this IApplicationBuilder app)
    {
        string rootFolder = FolderTypes.RootFolder.GetDescription();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), rootFolder);

        // Ensure the directory exists
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        return app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(filePath),
            RequestPath = new PathString("/" + rootFolder)
        });
    }
}
