using System.IO;
using System.Threading.Tasks;

namespace ContosoDashboard.Services.Storage;

public class NoOpFileScanner : IFileScanner
{
    public Task<bool> IsCleanAsync(Stream fileStream)
    {
        return Task.FromResult(true);
    }
}
