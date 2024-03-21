using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_MVC_Movie.Interfaces
{
    public interface ICommentService
    {
        Task<IActionResult> AddComment(int movieId, string commentText, ClaimsPrincipal user);
        Task<IActionResult> DeleteComment(int commentId, int movieId, ClaimsPrincipal user);
    }
}
