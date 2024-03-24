using ASP_MVC_Movie.Data;
using ASP_MVC_Movie.Interfaces;
using ASP_MVC_Movie.Models;
using ASP_MVC_Movie.ViewModel;
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
        private readonly IGenreService _genreService;
        private readonly IMovieService _movieService;
        private readonly IFileService _fileService;



        public MovieController(AppDbContext context, UserManager<AppUser> userManager, IGenreService genreService, IMovieService movieService, IFileService fileService)
        {
            _context = context;
            _userManager = userManager;
            _genreService = genreService;
            _movieService = movieService;
            _fileService = fileService;
        }


    

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 8)
        {
            IQueryable<Movie> allMovies = _context.Movies.Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre);
            ViewBag.Genres = await _genreService.GetAllGenres();

            return await Paginate(allMovies, pageIndex, pageSize, "Index");
        }

        public async Task<IActionResult> MoviesByGenre(int genreId, int pageIndex = 1, int pageSize = 8)
        {
            IQueryable<Movie> movies = _context.Movies.Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre).Where(m => m.MovieGenres.Any(mg => mg.GenreId == genreId));
            ViewBag.Genres = await _genreService.GetAllGenres();

            return await Paginate(movies, pageIndex, pageSize, "Index", genreId);
        }

        private async Task<IActionResult> Paginate(IQueryable<Movie> movies, int pageIndex, int pageSize, string viewName, int? genreId = null)
        {
            var totalMovies = await movies.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalMovies / pageSize);
            var paginatedMovies = await movies.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;
            ViewBag.GenreId = genreId;

            return View(viewName, paginatedMovies);
        }



        public IActionResult CreateGenre()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateGenre(GenreVM genreViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(genreViewModel);
            }

            var genre = new Genre { Name = genreViewModel.Name };
            await _genreService.CreateGenre(genre);   
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
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
        public async Task<IActionResult> UpdateGenre(int id, GenreVM genreViewModel)
        {
            if (id != genreViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(genreViewModel);
            }

            try
            {
                var genre = new Genre { Id = genreViewModel.Id, Name = genreViewModel.Name };
                await _genreService.UpdateGenre(genre);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_genreService.GenreExists(id))
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



        public IActionResult CreateMovie()
        {
            var viewModel = new MovieVM
            {
                AvailableGenres = _context.Genres
                    .Select(g => new GenreVM { Id = g.Id, Name = g.Name })
                    .ToList()
            };

            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieVM movieViewModel)
        {
            if (!ModelState.IsValid || movieViewModel.SelectedGenreIds == null)
            {
                movieViewModel.AvailableGenres = _context.Genres
                    .Select(g => new GenreVM { Id = g.Id, Name = g.Name })
                    .ToList();
                return View(movieViewModel);
            }

            var movie = new Movie
            {
                Title = movieViewModel.Title,
                Country = movieViewModel.Country,
                Rating = movieViewModel.Rating,
                Director = movieViewModel.Director,
                Description = movieViewModel.Description,
                ReleaseDate = movieViewModel.ReleaseDate
            };

            if (movieViewModel.VideoFile != null && movieViewModel.VideoFile.Length > 0)
            {
                movie.FilePath = await _fileService.SaveVideoAsync(movieViewModel.VideoFile);
            }

            movie.MovieGenres = movieViewModel.SelectedGenreIds
                .Select(genreId => new MovieGenre { GenreId = genreId })
                .ToList();

            await _movieService.CreateMovieAsync(movie);

            return RedirectToAction("Index", "Movie");
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

            var movieViewModel = new MovieVM
            {
                Id = movie.Id,
                Title = movie.Title,
                Country = movie.Country,
                Rating = movie.Rating,
                Director = movie.Director,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                AvailableGenres = genres.Select(g => new GenreVM { Id = g.Id, Name = g.Name }).ToList()
            };

            return View(movieViewModel);
        }



        [HttpPost]
        public async Task<IActionResult> UpdateMovie(int id, MovieVM movieViewModel)
        {
            if (id != movieViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
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

                return View(movieViewModel);
            }

            var movie = new Movie
            {
                Id = movieViewModel.Id,
                Title = movieViewModel.Title,
                Country = movieViewModel.Country,
                Rating = movieViewModel.Rating,
                Director = movieViewModel.Director,
                Description = movieViewModel.Description,
                ReleaseDate = movieViewModel.ReleaseDate
            };

            if (movieViewModel.VideoFile != null && movieViewModel.VideoFile.Length > 0)
            {
                movie.FilePath = await _fileService.SaveVideoAsync(movieViewModel.VideoFile);
            }

            var selectedGenreIds = movieViewModel.SelectedGenreIds?.ToList() ?? new List<int>();

            movie.MovieGenres = selectedGenreIds
                .Select(genreId => new MovieGenre { GenreId = genreId })
                .ToList();

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
