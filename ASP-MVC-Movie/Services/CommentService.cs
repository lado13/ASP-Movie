using ASP_MVC_Movie.Data;
using ASP_MVC_Movie.Interfaces;
using ASP_MVC_Movie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_MVC_Movie.Services
{
    public class CommentService : ICommentService
    {

        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;



        public CommentService(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddComment(int movieId, string commentText, ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return new RedirectToActionResult("Login", "Account", null);
            }

            if (string.IsNullOrWhiteSpace(commentText))
            {   
                return new RedirectToActionResult("DetailMovie", "Movie", new { id = movieId });
            }

            var currentUser = await _userManager.GetUserAsync(user);

            var comment = new Comment
            {
                Text = commentText,
                PostedAt = DateTime.Now,
                MovieId = movieId,
                UserId = currentUser.Id
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return new RedirectToActionResult("DetailMovie", "Movie", new { id = movieId });
        }


        public async Task<IActionResult> DeleteComment(int commentId, int movieId, ClaimsPrincipal user)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return new NotFoundResult();
            }

            if (!user.Identity.IsAuthenticated || (comment.UserId != user.FindFirstValue(ClaimTypes.NameIdentifier) && !user.IsInRole("Admin")))
            {
                return new ForbidResult();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return new RedirectToActionResult("DetailMovie", "Movie", new { id = movieId });
        }



    }
}
