using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MusicStreamDemo.API.Interface
{
    public interface IAzureBlobStorageService
    {
        Task<string> UploadMusicAsync(IFormFile file);
        Task<Stream> GetMusicStreamAsync(string fileName);
        Task<IEnumerable<string>> ListMusicAsync();
    }
}
