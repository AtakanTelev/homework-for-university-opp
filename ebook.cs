using System.Net;

namespace LibrarySystem
{
    public class EBook : Book
    {
        public string DownloadUrl { get; set; }
        public static int TotalDownloads = 0;

        public EBook(string title, string author, string downloadUrl)
            : base(title, author)
        {
            DownloadUrl = downloadUrl;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Download URL: {DownloadUrl}");
        }

        public async Task DownloadAsync()
        {
            string fileName = Title.Replace(" ", "_") + ".pdf";
            string downloadDir = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
            Directory.CreateDirectory(downloadDir);
            string filePath = Path.Combine(downloadDir, fileName);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    byte[] data = await client.GetByteArrayAsync(DownloadUrl);
                    await File.WriteAllBytesAsync(filePath, data);
                    Console.WriteLine($"\nDownloaded '{Title}' successfully.");
                    Console.WriteLine($"Saved to: {filePath}");
                    TotalDownloads++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to download '{Title}': {ex.Message}");
                }
            }
        }
    }
}
