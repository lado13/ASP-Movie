using ASP_MVC_Movie.Data;
using ASP_MVC_Movie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_Movie.Controllers
{
    public class MovieController : Controller
    {

        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;


        public MovieController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }





        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 8)
        {
            IQueryable<Movie> allMovies = _context.Movies.Include(m => m.Genre);
            ViewBag.Genres = await _context.Genres.ToListAsync(); 
            return await Paginate(allMovies, pageIndex, pageSize);
        }

        public async Task<IActionResult> MoviesByGenre(int genreId, int pageIndex = 1, int pageSize = 8)
        {
            IQueryable<Movie> movies = _context.Movies.Include(m => m.Genre).Where(m => m.GenreId == genreId);
            ViewBag.Genres = await _context.Genres.ToListAsync();
            return await Paginate(movies, pageIndex, pageSize);
        }


        private async Task<IActionResult> Paginate(IQueryable<Movie> movies, int pageIndex, int pageSize)
        {
            var totalMovies = await movies.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalMovies / pageSize);

            var paginatedMovies = await movies.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;

            return View("Index", paginatedMovies);
        }



        [HttpGet]
        public IActionResult Search(string query)
        {
            var searchResults = _context.Movies
                .Where(m => m.Title.Contains(query))
                .Select(m => new { m.Id, m.Title, m.FilePath }) 
                .ToList();
            return Json(searchResults);
        }




        public IActionResult CreateGenre()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateGenre([Bind("Id,Name")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }



      
        public async Task<IActionResult> UpdateGenre(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGenre(int id, [Bind("Id,Name")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }


        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }



        public async Task<IActionResult> RemoveGenre(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        public IActionResult CreateMovie()
        {
            ViewBag.Genres = _context.Genres.ToList();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateMovie(Movie movie)
        {
            if (ModelState.IsValid)
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
               

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }



        public IActionResult UpdateMovie(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            ViewBag.Genres = _context.Genres.ToList();
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMovie(int id, [Bind("Id,Title,GenreId,Country,Rating,Director,Description,ReleaseDate,VideoFile")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Genres = _context.Genres.ToList();
            return View(movie);
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }



        public async Task<IActionResult> RemoveMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);

            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> DetailMovie(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Comments)
                .ThenInclude(c => c.User) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }












    }
}
