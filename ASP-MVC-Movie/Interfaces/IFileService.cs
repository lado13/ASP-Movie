namespace ASP_MVC_Movie.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveVideoAsync(IFormFile videoFile);
    }
}
