using AnchorNews.Data;
using AnchorNews_AspNet.Models.Comments;
using AnchorNews_AspNet.Models.NewsPost;
using AnchorNews_AspNet.Models.UserAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
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

        public CommentController(ApplicationDbContext dbContext)
        {
            _context = dbContext;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(Guid postId)
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }
            var query = await _context.Comments.Where(c => c.NewsPostId == postId).ToListAsync();
            return query;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Editor, Guest")]
        public IActionResult AddComment(CommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the associated news post
            var newsPost = _context.NewsPosts.FirstOrDefault(np => np.Id == request.NewsPostId);

            if (newsPost == null)
            {
                return NotFound();
            }

            // Create a new comment
            var comment = new Comment
            {
                CommentText = request.CommentText,
                CommenterName = "User", //TODO get current user name,
                Timestamp = DateTime.UtcNow,
                NewsPost = newsPost
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Ok(JsonSerializer.Serialize(comment, _jsonSerializerOptions));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteComment(Guid id)
        {
            // Retrieve the comment from the database
            var comment = _context.Comments.FirstOrDefault(c => c.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
