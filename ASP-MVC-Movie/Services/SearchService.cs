using ASP_MVC_Movie.Data;
using ASP_MVC_Movie.Interfaces;
using ASP_MVC_Movie.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_Movie.Services
{
    public class SearchService : ISearchService
    {
        private readonly AppDbContext _context;


        public SearchService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Object>> SearchMoviesAsync(string query)
        {
            var searchResults = await _context.Movies
          .Where(m => m.Title.Contains(query))
          .Select(m => new { m.Id, m.Title, m.FilePath })
          .ToListAsync();

            return searchResults;
        }
    }
}
