using ASP_MVC_Movie.Data;
using ASP_MVC_Movie.Interfaces;
using ASP_MVC_Movie.Models;
using Microsoft.AspNetCore.Identity;

namespace ASP_MVC_Movie.Services
{
    public class MovieService : IMovieService
    {

        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public void AddMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public void DeleteMovie(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Movie> GetAllMovie()
        {
            throw new NotImplementedException();
        }

        public Movie GetByIdMovie(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
