using Microsoft.AspNetCore.Mvc;
using MusicStreamDemo.API.Interface;
using MusicStreamDemo.API.Services;
using MusicStreamDemo.Common;

namespace MusicStreamDemo.API.Controllers
{
    /// <summary>
    /// Controller for managing music uploads, streams, and metadata.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MusicController : ControllerBase
    {
        private readonly IAzureBlobStorageService _blobStorageService;
        private readonly IDataContext _dataContext;

        public MusicController(
            IAzureBlobStorageService blobStorageService,
            IDataContext dataContext)
        {
            _blobStorageService = blobStorageService;
            _dataContext = dataContext;
        }

        /// <summary>
        /// Uploads an MP3 file and stores its metadata.
        /// </summary>
        /// <param name="file">The MP3 file to upload.</param>
        /// <param name="title">The title of the song.</param>
        /// <param name="artist">The artist of the song.</param>
        /// <param name="album">The album of the song.</param>
        /// <returns>The uploaded song's metadata.</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string title, [FromForm] string artist, [FromForm] string album)
        {
            if (file == null || file.Length == 0 || Path.GetExtension(file.FileName)?.ToLower() != ".mp3")
                return BadRequest("Invalid file. Please upload an MP3 file.");

            try
            {
                var fileUrl = await _blobStorageService.UploadMusicAsync(file);
                if (fileUrl == null)
                    return StatusCode(500, "Failed to upload file to Azure Blob Storage.");

                TimeSpan duration;
                using (var stream = file.OpenReadStream())
                {
                    var fileAbstraction = new StreamFileAbstraction(file.FileName, stream);
                    var tagFile = TagLib.File.Create(fileAbstraction);
                    duration = tagFile.Properties.Duration;
                }

                var music = new Music
                {
                    FileName = file.FileName,
                    FileUrl = fileUrl,
                    Title = title,
                    Artist = artist,
                    Album = album,
                    Duration = duration,
                    UploadDate = DateTime.UtcNow
                };

                await _dataContext.AddMusicAsync(music);

                return Ok(music);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Streams an MP3 file from Azure Blob Storage.
        /// </summary>
        /// <param name="file">The name of the file to stream.</param>
        /// <returns>The audio file stream.</returns>
        [HttpGet("stream/{file}")]
        public async Task<IActionResult> Stream(string file)
        {
            try
            {
                var bytes = Convert.FromBase64String(file);
                var fileName = System.Text.Encoding.UTF8.GetString(bytes);

                var stream = await _blobStorageService.GetMusicStreamAsync(fileName);
                return File(stream, "audio/mpeg", enableRangeProcessing: true);
            }
            catch (FileNotFoundException)
            {
                return NotFound("File not found in Azure Blob Storage.");
            }
        }

        /// <summary>
        /// Retrieves a list of all uploaded songs.
        /// </summary>
        /// <returns>A list of song metadata.</returns>
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            try
            {
                var musicList = await _dataContext.GetAllMusicAsync();
                return Ok(musicList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
