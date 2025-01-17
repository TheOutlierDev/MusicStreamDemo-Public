using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MusicStreamDemo.API.Interface;

namespace MusicStreamDemo.API.Services
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "musicstreamdemocontainer";

        public AzureBlobStorageService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadMusicAsync(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(file.FileName);

            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, true);

            return blobClient.Uri.ToString();
        }

        public async Task<Stream> GetMusicStreamAsync(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                return await blobClient.OpenReadAsync();
            }

            throw new FileNotFoundException("File not found in Azure Blob Storage.");
        }

        public async Task<IEnumerable<string>> ListMusicAsync()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobs = containerClient.GetBlobsAsync();

            var musicList = new List<string>();
            await foreach (var blobItem in blobs)
            {
                musicList.Add(blobItem.Name);
            }

            return musicList;
        }


    }
}
