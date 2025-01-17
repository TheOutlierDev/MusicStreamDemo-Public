namespace MusicStreamDemo.Common
{
    public class Music
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public string? Title { get; set; }

        public string? Artist { get; set; }

        public string? Album { get; set; }

        public TimeSpan? Duration { get; set; }

        public DateTime UploadDate { get; set; }

        public string? Tags { get; set; }

    }
}
