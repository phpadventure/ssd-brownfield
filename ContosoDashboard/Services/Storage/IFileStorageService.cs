using System.IO;
using System.Threading.Tasks;

namespace ContosoDashboard.Services.Storage;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    Task DeleteAsync(string storagePath);
    Task<Stream> DownloadAsync(string storagePath);
}
