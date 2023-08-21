using AnchorNews.Data;
using AnchorNews_AspNet.Models.ApiNewsPost;
using AnchorNews_AspNet.Models.NewsPost;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnchorNews_AspNet.Services
{
    public class NewsPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly NewsApiService _newsApiService;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public NewsPostService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, NewsApiService newsApiService)
        {
            _context = dbContext;
            _newsApiService = newsApiService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ActionResult<Post>> CreateNewsPost(NewsPostCommandRequest request)
        {
            var existingBrekingNews = await _context.NewsPosts.Where(x => x.IsBreakingNews).FirstOrDefaultAsync();

            if (existingBrekingNews is not null && request.IsBreakingNews)
            {
                existingBrekingNews.IsBreakingNews = false;
                existingBrekingNews.BreakingNewsExpiration = null;
            }
            Post post = new Post
            {
                Headline = request.Headline,
                ShortDescription = request.ShortDescription,
                FullDescription = request.FullDescription,
                ImageUrl = request.ImageUrl,
                Category = request.Category,
                IsBreakingNews = request.IsBreakingNews,
                BreakingNewsExpiration = request.IsBreakingNews ? DateTime.Now.AddHours(48) : null

            };
            _context.NewsPosts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<ActionResult<IEnumerable<Post>>> GetAllNewsPosts()
        {
            var query = await _context.NewsPosts.ToListAsync();
            foreach (var item in query)
            {
                if (IsBreakingNewsExpired(item))
                {
                    item.IsBreakingNews = false;
                    item.BreakingNewsExpiration = null;
                }
            }

            return query;


        }

        public async Task<ActionResult<Post>> GetNewsPostById(Guid id)
        {
            var newsPost = await _context.NewsPosts.FindAsync(id);

            var user = _httpContextAccessor.HttpContext.User;

            if (newsPost == null)
            {
                return new NotFoundResult();
            }
            if (IsBreakingNewsExpired(newsPost))
            {
                newsPost.IsBreakingNews = false;
                newsPost.BreakingNewsExpiration = null;
            }

            //Admins and editors should not increment post views 
            if (!user.IsInRole("Admin") && !user.IsInRole("Editor"))
            {
                IncrementViewCount(newsPost.Id);
            }

            return newsPost;

        }

        public async Task<IActionResult> EditNewsPost(Guid id, NewsPostCommandRequest request)
        {
            if (_context.NewsPosts == null)
            {
                return new BadRequestResult();
            }
            var query = await _context.NewsPosts.FirstOrDefaultAsync(x => x.Id == id);


            var existingBrekingNews = request.IsBreakingNews ? await _context.NewsPosts.FirstOrDefaultAsync(x => x.IsBreakingNews) : null;

            if (query is null)
            {
                return new NotFoundResult();
            }

            if (!Equals(request.Headline, query.Headline))
            {
                query.Headline = request.Headline;
            }

            if (!Equals(request.ShortDescription, query.ShortDescription))
            {
                query.ShortDescription = request.ShortDescription;
            }

            if (!Equals(request.FullDescription, query.FullDescription))
            {
                query.FullDescription = request.FullDescription;
            }

            if (!Equals(request.ImageUrl, query.ImageUrl))
            {
                query.ImageUrl = request.ImageUrl;
            }

            if (!Equals(request.Category, query.Category))
            {
                query.Category = request.Category;
            }

            if (!Equals(request.IsBreakingNews, query.IsBreakingNews))
            {
                query.IsBreakingNews = request.IsBreakingNews;
                query.BreakingNewsExpiration = request.IsBreakingNews ? DateTime.Now.AddHours(48) : null;

                if (existingBrekingNews is not null)
                {
                    existingBrekingNews.IsBreakingNews = false;
                    existingBrekingNews.BreakingNewsExpiration = null;
                }
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> DeleteNewsPost(Guid id)
        {

            if (_context.NewsPosts == null)
            {
                return new BadRequestResult();
            }
            var newsPost = await _context.NewsPosts.FindAsync(id);
            if (newsPost == null)
            {
                return new NotFoundResult();
            }

            _context.NewsPosts.Remove(newsPost);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        //EXTERNAL API METHODS
        public async Task<IActionResult> GetNewsFromApi()
        {

            if (_context.NewsPosts is null)
            {
                return new BadRequestResult();
            }
            var apiNewsPosts = await _newsApiService.FetchNewsPostsAsync();

            foreach (var post in apiNewsPosts.Articles)
            {
                var apiNewsPost = new Post
                {
                    Headline = post.Title,
                    ShortDescription = !string.IsNullOrEmpty(post.Description) ? post.Description : "",
                    FullDescription = !string.IsNullOrEmpty(post.Content) ? post.Content : "",
                    ImageUrl = !string.IsNullOrEmpty(post.UrlToImage) ? post.UrlToImage : "",
                    Category = "API",
                    IsBreakingNews = false
                };

                _context.NewsPosts.Add(apiNewsPost);
            }
            _context.SaveChangesAsync();


            return new OkResult();
        }

        public async Task<ActionResult<IEnumerable<ApiPost>>> GetFetchedNews()
        {
            if (_context.ApiNewsPosts == null)
            {
                return new BadRequestResult();
            }
            var query = await _context.ApiNewsPosts.ToListAsync();
            return new OkObjectResult(query);

        }


        //HELPER METHODS
        private bool IsBreakingNewsExpired(Post newsPost)
        {
            if (newsPost.IsBreakingNews && newsPost.BreakingNewsExpiration.HasValue)
            {
                return newsPost.BreakingNewsExpiration.Value <= DateTime.UtcNow;
            }

            return false;
        }

        private void IncrementViewCount(Guid newsPostId)
        {
            var newsPost = _context.NewsPosts.FirstOrDefault(np => np.Id == newsPostId);
            if (newsPost != null)
            {
                newsPost.ViewCount++;
                _context.SaveChanges();
            }
        }


    }
}
