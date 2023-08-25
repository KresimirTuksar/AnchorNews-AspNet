using AnchorNews.Models;

namespace AnchorNews.Web.Services
{
    public interface ICommentsService
    {
        Task<IEnumerable<Comment>> GetCommentsAsync(string postId);
        Task AddComment(CommentRequest commentRequest);
    }
}
