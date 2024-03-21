using ASP_MVC_Movie.Data;
using ASP_MVC_Movie.Interfaces;
using ASP_MVC_Movie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_Movie.Controllers
{
    public class MovieController : Controller
    {

        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IGenreService _genreService;
        private readonly IMovieService _movieService;


        public MovieController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment, IGenreService genreService, IMovieService movieService)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
            _genreService = genreService;
            _movieService = movieService;
        }


        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 8)
        {
            IQueryable<Movie> allMovies = _context.Movies.Include(m => m.Genre);
            ViewBag.Genres = await _genreService.GetAllGenres();
            return await Paginate(allMovies, pageIndex, pageSize);
        }

        public async Task<IActionResult> MoviesByGenre(int genreId, int pageIndex = 1, int pageSize = 8)
        {
            IQueryable<Movie> movies = _context.Movies.Include(m => m.Genre).Where(m => m.GenreId == genreId);
            ViewBag.Genres = await _genreService.GetAllGenres();
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


        public IActionResult CreateGenre()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateGenre(Genre genre)
        {
            if (ModelState.IsValid)
            {
                await _genreService.CreateGenre(genre);
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

            var genre = await _genreService.GetGenreById(id.Value);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGenre(int id, Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _genreService.UpdateGenre(genre);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_genreService.GenreExists(genre.Id))
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

        public async Task<IActionResult> RemoveGenre(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _genreService.RemoveGenre(id.Value);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CreateMovie()
        {

            var genres = await _genreService.GetAllGenres();
            if (genres == null)
            {
                genres = new List<Genre>();
            }

            var genreSelectList = genres.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            }).ToList();

            ViewBag.Genres = genreSelectList;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateMovie(Movie movie)
        {

            var genres = await _genreService.GetAllGenres();
            if (genres == null)
            {
                genres = new List<Genre>();
            }

            var genreSelectList = genres.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            }).ToList();

            ViewBag.Genres = genreSelectList;


            if (ModelState.IsValid)
            {
                await _movieService.CreateMovieAsync(movie);
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }

        public async Task<IActionResult> UpdateMovie(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var genres = await _genreService.GetAllGenres();
            if (genres == null)
            {
                genres = new List<Genre>();
            }

            var genreSelectList = genres.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            }).ToList();

            ViewBag.Genres = genreSelectList;
            return View(movie);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            var genres = await _genreService.GetAllGenres();
            if (genres == null)
            {
                genres = new List<Genre>();
            }

            var genreSelectList = genres.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            }).ToList();

            ViewBag.Genres = genreSelectList;

            if (ModelState.IsValid)
            {
                try
                {
                    await _movieService.UpdateMovieAsync(movie);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_movieService.MovieExists(movie.Id))
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
 
            return View(movie);
        }

       
        public async Task<IActionResult> RemoveMovie(int id)
        {
            await _movieService.RemoveMovieAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DetailMovie(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieService.GetMovieDetailsAsync(id.Value);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }



    }
}
