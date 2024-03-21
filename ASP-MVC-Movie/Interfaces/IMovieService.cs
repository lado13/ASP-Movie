using ASP_MVC_Movie.Models;

namespace ASP_MVC_Movie.Interfaces
{
    public interface IMovieService
    {
        Task<Movie> GetMovieByIdAsync(int id);
        Task<List<Movie>> GetAllMoviesAsync();
        Task CreateMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);
        Task RemoveMovieAsync(int id);
        Task<Movie> GetMovieDetailsAsync(int id);
        bool MovieExists(int id);
    }
}
