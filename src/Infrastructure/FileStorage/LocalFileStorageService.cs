using Microsoft.Teams.Assist.Application.Common.Extensions;
using Microsoft.Teams.Assist.Application.Common.FileStorage;
using Microsoft.Teams.Assist.Domain.Enums;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Microsoft.Teams.Assist.Infrastructure.FileStorage;

public class LocalFileStorageService : IFileStorageService
{
    private string GetRootDirectory()
    {
        return Directory.GetCurrentDirectory();
    }

    private string GetFolderName<T>(FileType supportedFileType)
        where T : class
    {
        string folder = typeof(T).Name;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            folder = folder.Replace(@"\", "/");
        }

        return supportedFileType switch
        {
            FileType.Image => Path.Combine("Files", "Images", folder),
            _ => Path.Combine("Files", "Others", folder),
        };
    }

    public async Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
    where T : class
    {
        if (request == null || request.Data == null)
        {
            return string.Empty;
        }

        if (string.IsNullOrEmpty(request.Data))
            throw new InvalidOperationException("Please Upload a file.");
        if (request.Extension is null || !supportedFileType.GetDescriptionList().Contains(request.Extension.ToLower()))
            throw new InvalidOperationException("File Format Not Supported.");
        if (request.Name is null)
            throw new InvalidOperationException("Name is required.");
        string base64Data = string.Empty;
        if (supportedFileType == FileType.Image)
        {
            base64Data = Regex.Match(request.Data, "data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
        }
        else if (supportedFileType == FileType.Document)
        {
            base64Data = request.Data;
        }
        else if (supportedFileType == FileType.AllValidDocuments)
        {
            base64Data = request.Data;
        }

        var streamData = new MemoryStream(Convert.FromBase64String(base64Data));
        if (streamData.Length > 0)
        {
            //string folder = typeof(T).Name;
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            //{
            //    folder = folder.Replace(@"\", "/");
            //}

            //string folderName = supportedFileType switch
            //{
            //    FileType.Image => Path.Combine("Files", "Images", folder),
            //    _ => Path.Combine("Files", "Others", folder),
            //};
            string folderName = GetFolderName<T>(supportedFileType);
            string pathToSave = Path.Combine(GetRootDirectory(), folderName);
            Directory.CreateDirectory(pathToSave);

            string fileName = request.Name.Trim('"');
            fileName = RemoveSpecialCharacters(fileName);
            fileName = fileName.ReplaceWhitespace("-");
            fileName += request.Extension.Trim();
            string fullPath = Path.Combine(pathToSave, fileName);
            string dbPath = Path.Combine(folderName, fileName);
            if (File.Exists(dbPath))
            {
                dbPath = NextAvailableFilename(dbPath);
                fullPath = NextAvailableFilename(fullPath);
            }

            using var stream = new FileStream(fullPath, FileMode.Create);
            await streamData.CopyToAsync(stream, cancellationToken);
            return dbPath.Replace("\\", "/");
        }
        else
        {
            return string.Empty;
        }
    }

    public static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
    }

    public void Remove(string? path)
    {
        path = Path.Combine(GetRootDirectory(), path);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private const string NumberPattern = "-{0}";

    private static string NextAvailableFilename(string path)
    {
        if (!File.Exists(path))
        {
            return path;
        }

        if (Path.HasExtension(path))
        {
            return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal), NumberPattern));
        }

        return GetNextFilename(path + NumberPattern);
    }

    private static string GetNextFilename(string pattern)
    {
        string tmp = string.Format(pattern, 1);

        if (!File.Exists(tmp))
        {
            return tmp;
        }

        int min = 1, max = 2;

        while (File.Exists(string.Format(pattern, max)))
        {
            min = max;
            max *= 2;
        }

        while (max != min + 1)
        {
            int pivot = (max + min) / 2;
            if (File.Exists(string.Format(pattern, pivot)))
            {
                min = pivot;
            }
            else
            {
                max = pivot;
            }
        }

        return string.Format(pattern, max);
    }

    public bool ValidateFiles(List<FileUploadRequest> requests, FileType supportedFileType)
    {
        foreach (var request in requests)
        {
            if (string.IsNullOrEmpty(request.Data))
                throw new InvalidOperationException("Please Upload a file.");
            if (request.Extension is null || !supportedFileType.GetDescriptionList().Contains(request.Extension.ToLower()))
                throw new InvalidOperationException("File Format Not Supported for " + request.Name);
            if (request.Name is null)
                throw new InvalidOperationException("Name is required.");
        }

        return true;
    }

    public string FileToBase64String(string path)
    {
        path = Path.Combine(GetRootDirectory(), path);
        if (File.Exists(path))
        {
            byte[] imageArray = File.ReadAllBytes(path);
            return Convert.ToBase64String(imageArray);
        }

        return string.Empty;
    }
}
