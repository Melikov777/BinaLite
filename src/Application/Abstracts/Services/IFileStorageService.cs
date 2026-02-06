namespace Application.Abstracts.Services;

public interface IFileStorageService
{
    Task<string> SaveAsync(Stream content, string fileName, string contentType, int propertyAdId, CancellationToken ct = default);
    Task DeleteFileAsync(string objectKey, CancellationToken ct = default);
    string GetFileUrl(string fileName); // keeping this for now as it's used in resolver, though not in screenshot requirements, might need to keep it or move logic.
}
