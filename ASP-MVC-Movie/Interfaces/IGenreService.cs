using ASP_MVC_Movie.Models;

namespace ASP_MVC_Movie.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllGenres();
        Task<Genre> GetGenreById(int id);
        Task CreateGenre(Genre genre);
        Task UpdateGenre(Genre genre);
        Task<bool> RemoveGenre(int id);
        bool GenreExists(int id);
    }
}
