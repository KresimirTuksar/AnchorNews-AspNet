using AnchorNews.Data;
using AnchorNews_AspNet.Models.Comments;
using AnchorNews_AspNet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AnchorNews_AspNet.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly CommentService _commentService;

        public CommentController(ApplicationDbContext dbContext, CommentService commentService)
        {
            _context = dbContext;
            _commentService = commentService;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(Guid postId)
        {
            var result = await _commentService.Getcomments(postId);
            if (result is null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(CommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUser = User.Identity.Name;

            var result = _commentService.AddComment(request, currentUser);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var result = await _commentService.DeleteComment(id);

            if (result.Equals(false))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
