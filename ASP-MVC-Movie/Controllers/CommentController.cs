using ASP_MVC_Movie.Data;
using ASP_MVC_Movie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_MVC_Movie.Controllers
{
    public class CommentController : Controller
    {


        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public CommentController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }





        [HttpPost]
        public async Task<IActionResult> AddComment(int movieId, string commentText)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrWhiteSpace(commentText))
            {
                TempData["ErrorMessage"] = "Please enter a comment.";
                return RedirectToAction("DetailMovie", "Movie", new { id = movieId });
            }

            var user = await _userManager.GetUserAsync(User);

            var comment = new Comment
            {
                Text = commentText,
                PostedAt = DateTime.Now,
                MovieId = movieId,
                UserId = user.Id
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("DetailMovie", "Movie", new { id = movieId });
        }



        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId, int movieId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound(); 
            }

            if (!User.Identity.IsAuthenticated || (comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin")))
            {
                return Forbid();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("DetailMovie","Movie", new { id = movieId });
        }










    }
}
