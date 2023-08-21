using AnchorNews.Models;

namespace AnchorNews.Web.Services
{
    public interface IAnchorNewsService
    {
        Task<IEnumerable<Post>> GetAllPosts();
        Task<Post> GetPostDetails(string postId);
    }
}
