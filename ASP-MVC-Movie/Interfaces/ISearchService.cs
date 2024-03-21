using ASP_MVC_Movie.Models;

namespace ASP_MVC_Movie.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<object>> SearchMoviesAsync(string query);
    }
}
