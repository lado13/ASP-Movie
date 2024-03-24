using ASP_MVC_Movie.Interfaces;

namespace ASP_MVC_Movie.Services
{
    public class FileService : IFileService
    {

        private readonly string _uploadsFolder;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        }

        public async Task<string> SaveVideoAsync(IFormFile videoFile)
        {
            if (videoFile == null || videoFile.Length == 0)
            {
                throw new ArgumentException("Video file is missing or empty.");
            }

            var videosFolder = Path.Combine(_uploadsFolder, "videos");

            if (!Directory.Exists(videosFolder))
            {
                Directory.CreateDirectory(videosFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + videoFile.FileName;
            var filePath = Path.Combine(videosFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await videoFile.CopyToAsync(fileStream);
            }

            return "/uploads/videos/" + uniqueFileName;
        }




    }
}
