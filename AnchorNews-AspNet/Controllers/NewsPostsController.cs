using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnchorNews.Data;
using AnchorNews_AspNet.Models.NewsPost;
using AnchorNews_AspNet.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace AnchorNews_AspNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsPostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly NewsPostService _newsPostService;

        public NewsPostsController(ApplicationDbContext context)
        {
            _context = context;
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
                if (item.BreakingNewsExpiration < DateTime.Now)
                {
                    item.IsBreakingNews = false;
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

            return newsPost;
        }

        // PUT: api/NewsPosts/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin, Editor, Guest")]
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
        //[Authorize(Roles = "Admin, Editor, Guest")]
        public async Task<ActionResult<Post>> PostNewsPost(NewsPostCommandRequest command)
        {
          if (_context.NewsPosts == null)
          {
              return Problem("Entity set 'ApplicationDbContext.NewsPosts'  is null.");
          }
            var existingBrekingNews = await _context.NewsPosts.Where(x => x.IsBreakingNews).FirstOrDefaultAsync();

            if (existingBrekingNews is not null && command.IsBreakingNews) { 
                existingBrekingNews.IsBreakingNews = false;
                existingBrekingNews.BreakingNewsExpiration = null;
            }
            Post post = new Post
            {
                Id = new Guid(),
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
        //[Authorize(Roles = "Admin, Editor, Guest")]
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

        private bool NewsPostExists(Guid id)
        {
            return (_context.NewsPosts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
