using System.IO;
using System.Threading.Tasks;

namespace ContosoDashboard.Services.Storage;

public interface IFileScanner
{
    Task<bool> IsCleanAsync(Stream fileStream);
}
