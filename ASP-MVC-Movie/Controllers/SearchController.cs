using ASP_MVC_Movie.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASP_MVC_Movie.Controllers
{
    public class SearchController : Controller
    {

        private readonly ISearchService _searchService;


        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }


        [HttpGet]
        public async Task<IActionResult> SearchByTitle(string query)
        {
            var searchResults = await _searchService.SearchMoviesAsync(query);
            return Json(searchResults);
        }

    }
}
