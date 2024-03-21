using ASP_MVC_Movie.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASP_MVC_Movie.Controllers
{
    public class CommentController : Controller
    {

        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }


        [HttpPost]
        public async Task<IActionResult> AddComment(int movieId, string commentText)
        {
            return await _commentService.AddComment(movieId, commentText, User);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId, int movieId)
        {
            return await _commentService.DeleteComment(commentId, movieId, User);
        }

    }
}
