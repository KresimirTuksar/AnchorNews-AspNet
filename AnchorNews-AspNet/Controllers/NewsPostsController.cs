using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnchorNews.Data;
using AnchorNews_AspNet.Models.NewsPost;
using AnchorNews_AspNet.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using AnchorNews_AspNet.Models.ApiNewsPost;
using System.Collections.Specialized;

namespace AnchorNews_AspNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly NewsPostService _newsPostService;
        private readonly NewsApiService _newsApiService;

        public NewsPostsController(ApplicationDbContext context, NewsApiService newsApiService)
        {
            _context = context;
            _newsApiService = newsApiService;
        }

        // GET: api/NewsPosts
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Post>>> GetNewsPosts()
        {
          if (_context.NewsPosts == null)
          {
              return NotFound();
          }
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

        // GET: api/NewsPosts/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Post>> GetNewsPost(Guid id)
        {
          if (_context.NewsPosts == null)
          {
              return NotFound();
          }
            var newsPost = await _context.NewsPosts.FindAsync(id);

            if (newsPost == null)
            {
                return NotFound();
            }

            if (IsBreakingNewsExpired(newsPost))
            {
                newsPost.IsBreakingNews = false;
                newsPost.BreakingNewsExpiration = null;
            }

                IncrementViewCount(newsPost.Id);

            return newsPost;
        }

        // PUT: api/NewsPosts/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Editor")]

        public async Task<IActionResult> PutNewsPost(Guid id, NewsPostCommandRequest command)
        {
            var query = await _context.NewsPosts.FirstOrDefaultAsync(x => x.Id == id);

            var existingBrekingNews = command.IsBreakingNews ? await _context.NewsPosts.FirstOrDefaultAsync(x => x.IsBreakingNews) : null;
            
            if (query is null)
            {
                return BadRequest();
            }

            if (!Equals(command.Headline, query.Headline))
            {
                query.Headline = command.Headline;
            }

            if (!Equals(command.ShortDescription, query.ShortDescription))
            {
                query.ShortDescription = command.ShortDescription;
            }

            if (!Equals(command.FullDescription, query.FullDescription))
            {
                query.FullDescription = command.FullDescription;
            }

            if (!Equals(command.ImageUrl, query.ImageUrl))
            {
                query.ImageUrl = command.ImageUrl;
            }

            if (!Equals(command.Category, query.Category))
            {
                query.Category = command.Category;
            }

            if (!Equals(command.IsBreakingNews, query.IsBreakingNews))
            {
                query.IsBreakingNews = command.IsBreakingNews;
                query.BreakingNewsExpiration = command.IsBreakingNews ? DateTime.Now.AddHours(48) : null;

                if (existingBrekingNews is not null)
                {
                    existingBrekingNews.IsBreakingNews = false;
                    existingBrekingNews.BreakingNewsExpiration = null;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsPostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/NewsPosts
        [HttpPost]
        [Authorize(Roles = "Admin, Editor")]
        public async Task<ActionResult<Post>> PostNewsPost(NewsPostCommandRequest command)
        {
            if (_context.NewsPosts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.NewsPosts'  is null.");
            }

            var user = User;
            var existingBrekingNews = await _context.NewsPosts.Where(x => x.IsBreakingNews).FirstOrDefaultAsync();

            if (existingBrekingNews is not null && command.IsBreakingNews) { 
                existingBrekingNews.IsBreakingNews = false;
                existingBrekingNews.BreakingNewsExpiration = null;
            }
            Post post = new Post
            {
                Headline = command.Headline,
                ShortDescription = command.ShortDescription,
                FullDescription = command.FullDescription,
                ImageUrl = command.ImageUrl,
                Category = command.Category,
                IsBreakingNews = command.IsBreakingNews,
                BreakingNewsExpiration = command.IsBreakingNews ? DateTime.Now.AddHours(48) : null

            };
            _context.NewsPosts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNewsPost", new { id = post.Id }, post);
        }

        // DELETE: api/NewsPosts/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNewsPost(Guid id)
        {
            if (_context.NewsPosts == null)
            {
                return NotFound();
            }
            var newsPost = await _context.NewsPosts.FindAsync(id);
            if (newsPost == null)
            {
                return NotFound();
            }

            _context.NewsPosts.Remove(newsPost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [Route("GetApiNews")]

        [Authorize(Roles = "Admin, Editor")]
        public async Task<IActionResult> GetNewsFromApi()
        {
            var apiNewsPosts = await _newsApiService.FetchNewsPostsAsync();

            foreach (var post in apiNewsPosts)
            {
                var apiNewsPost = new ApiPost
                {
                    Headline = post.Title,
                    ShortDescription = !string.IsNullOrEmpty(post.Description) ? post.Description : "" ,
                    FullDescription = !string.IsNullOrEmpty(post.Content) ? post.Content : "",
                    ImageUrl = !string.IsNullOrEmpty(post.UrlToImage) ? post.UrlToImage : "",
                    Category = "API",
                    IsBreakingNews = false
                };

                _context.ApiNewsPosts.Add(apiNewsPost);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Editor")]
        [Route("GetFetchedNews")]

        public async Task<ActionResult<IEnumerable<ApiPost>>> GetFetchedNews()
        {
            if (_context.ApiNewsPosts == null)
            {
                return NotFound();
            }
            var query = await _context.ApiNewsPosts.ToListAsync();
            return query;
        }


        //Hepler Methods
        private bool NewsPostExists(Guid id)
        {
            return (_context.NewsPosts?.Any(e => e.Id == id)).GetValueOrDefault();
        }

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
