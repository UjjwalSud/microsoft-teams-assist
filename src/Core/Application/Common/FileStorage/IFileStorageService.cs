namespace Microsoft.Teams.Assist.Application.Common.FileStorage;

public interface IFileStorageService : ITransientService
{
    public Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
    where T : class;

    public bool ValidateFiles(List<FileUploadRequest> request, FileType supportedFileType);

    public void Remove(string? path);

    public string FileToBase64String(string path);
}
