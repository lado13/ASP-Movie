using ASP_MVC_Movie.Data;
using ASP_MVC_Movie.Interfaces;
using ASP_MVC_Movie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_Movie.Services
{
    public class MovieService : IMovieService
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public MovieService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task CreateMovieAsync(Movie movie)
        {
            //if (movie.VideoFile != null && movie.VideoFile.Length > 0)
            //{
            //    var uploadsFolder = Path.Combine(_environment.WebRootPath, "videos");

            //    if (!Directory.Exists(uploadsFolder))
            //    {
            //        Directory.CreateDirectory(uploadsFolder);
            //    }

            //    var uniqueFileName = Guid.NewGuid().ToString() + "_" + movie.VideoFile.FileName;
            //    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            //    using (var fileStream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await movie.VideoFile.CopyToAsync(fileStream);
            //    }

            //    movie.FilePath = "/videos/" + uniqueFileName;
            //}
            _context.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            if (movie.VideoFile != null && movie.VideoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "videos");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + movie.VideoFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await movie.VideoFile.CopyToAsync(fileStream);
                }

                movie.FilePath = "/videos/" + uniqueFileName;
            }
            _context.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }




        public async Task<Movie> GetMovieDetailsAsync(int id)
        {
            return await _context.Movies
                .Include(m => m.MovieGenres) 
                    .ThenInclude(mg => mg.Genre)
                .Include(m => m.Comments)
                    .ThenInclude(c => c.User) 
                .FirstOrDefaultAsync(m => m.Id == id);
        }








        //public async Task<Movie> GetMovieDetailsAsync(int id)
        //{
        //    return await _context.Movies
        //        .Include(m => m.Genre)
        //        .Include(m => m.Comments)
        //        .ThenInclude(c => c.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //}

        public bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
