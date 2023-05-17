using AnchorNews.Data;
using AnchorNews_AspNet.Models.NewsPost;
using Microsoft.EntityFrameworkCore;

namespace AnchorNews_AspNet.Services
{
    public class NewsPostService
    {
        private readonly ApplicationDbContext _dbContext;

        public NewsPostService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Post CreateNewsPost(Post newsPost)
        {
            // TODO: Validate and sanitize the input data for news post creation
            // TODO: Perform authorization checks for creating news posts

            _dbContext.NewsPosts.Add(newsPost);
            _dbContext.SaveChanges();

            return newsPost;
        }

        //public IEnumerable<Post> GetNewsPosts()
        //{
        //    // TODO: Retrieve and return the list of news posts

        //}

        //public Post GetNewsPostById(int id)
        //{
        //     TODO: Retrieve and return a news post by its ID
        //}

        //public Post UpdateNewsPost(int id, Post newsPost)
        //{
        //    // TODO: Validate and sanitize the input data for news post update
        //    // TODO: Perform authorization checks for updating news posts

        //    var existingNewsPost = _dbContext.NewsPosts.FirstOrDefault(np => np.Id == id);

        //    if (existingNewsPost != null)
        //    {
        //        existingNewsPost.Headline = newsPost.Headline;
        //        existingNewsPost.ShortDescription = newsPost.ShortDescription;
        //        existingNewsPost.FullDescription = newsPost.FullDescription;
        //        existingNewsPost.ImageUrl = newsPost.ImageUrl;
        //        existingNewsPost.Category = newsPost.Category;
        //        existingNewsPost.IsBreakingNews = newsPost.IsBreakingNews;

        //        _dbContext.SaveChanges();

        //        return existingNewsPost;
        //    }

        //    return null;
        //}

        //public Post DeleteNewsPost(int id)
        //{
        //    // TODO: Perform authorization checks for deleting news posts

        //    var existingNewsPost = _dbContext.NewsPosts.FirstOrDefault(np => np.Id == id);

        //    if (existingNewsPost != null)
        //    {
        //        _dbContext.NewsPosts.Remove(existingNewsPost);
        //        _dbContext.SaveChanges();

        //        return existingNewsPost;
        //    }

        //    return null;
        //}
    }
}
