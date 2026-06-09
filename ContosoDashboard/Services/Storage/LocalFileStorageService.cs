using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ContosoDashboard.Services.Storage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _basePath = configuration["FileStorage:BasePath"] ?? Path.Combine("AppData","uploads");
        if (!Path.IsPathRooted(_basePath))
        {
            _basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), _basePath));
        }
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var guid = System.Guid.NewGuid().ToString();
        var ext = Path.GetExtension(fileName);
        var storageName = guid + ext;
        var storagePath = Path.Combine(_basePath, storageName);
        using var outStream = File.Create(storagePath);
        fileStream.Position = 0;
        await fileStream.CopyToAsync(outStream);
        return storageName;
    }

    public Task DeleteAsync(string storagePath)
    {
        var full = Path.Combine(_basePath, storagePath);
        if (File.Exists(full)) File.Delete(full);
        return Task.CompletedTask;
    }

    public Task<Stream> DownloadAsync(string storagePath)
    {
        var full = Path.Combine(_basePath, storagePath);
        Stream stream = File.OpenRead(full);
        return Task.FromResult(stream);
    }
}
