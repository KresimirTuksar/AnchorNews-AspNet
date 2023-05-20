using AnchorNews.Data;
using AnchorNews_AspNet.Models.Comments;
using AnchorNews_AspNet.Models.NewsPost;
using AnchorNews_AspNet.Models.UserAuth;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AnchorNews_AspNet.Services
{
    public class CommentService
    {
        private readonly ApplicationDbContext _context;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public CommentService(ApplicationDbContext dbContext) {

            _context = dbContext;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
        }

        public async Task<ActionResult<IEnumerable<Comment>>> Getcomments(Guid postId)
        {
            if (_context.Comments == null)
            {
                return null;
            }
            var query = await _context.Comments.Where(c => c.NewsPostId == postId).OrderByDescending(c => c.Timestamp).ToListAsync();
            return query;
        }

        public async Task<ActionResult<Comment>> AddComment(CommentRequest request, string currentUser)
        {

            var newsPost = await _context.NewsPosts.FirstOrDefaultAsync(np => np.Id == request.NewsPostId);

            if (newsPost == null)
            {
                return null;
            }

            var comment = new Comment
            {
                CommentText = request.CommentText,
                CommenterName = currentUser,
                Timestamp = DateTime.UtcNow,
                NewsPost = newsPost
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();
            return comment;
        }

        public async Task<ActionResult<bool>> DeleteComment(Guid request)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == request);
            if (comment == null)
            {
                return false;
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return true; 
        }
    }
}
