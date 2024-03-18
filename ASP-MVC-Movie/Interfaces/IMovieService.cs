using ASP_MVC_Movie.Models;

namespace ASP_MVC_Movie.Interfaces
{
    public interface IMovieService
    {
        void AddMovie(Movie movie);
        IEnumerable<Movie> GetAllMovie();
        Movie GetByIdMovie(int id);
        void UpdateMovie(Movie movie);
        void DeleteMovie(int id);

    }
}
