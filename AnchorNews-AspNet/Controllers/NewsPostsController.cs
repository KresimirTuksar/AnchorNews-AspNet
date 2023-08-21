using Microsoft.AspNetCore.Mvc;
using AnchorNews.Data;
using AnchorNews_AspNet.Models.NewsPost;
using AnchorNews_AspNet.Services;
using Microsoft.AspNetCore.Authorization;
using AnchorNews_AspNet.Models.ApiNewsPost;

namespace AnchorNews_AspNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly NewsPostService _newsPostService;
        private readonly NewsApiService _newsApiService;

        public NewsPostsController(ApplicationDbContext context, NewsPostService newspostService, NewsApiService newsApiService)
        {
            _context = context;
            _newsPostService = newspostService;
            _newsApiService = newsApiService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getAllNewsPost")]
        public async Task<ActionResult<IEnumerable<Post>>> GetAllNewsPosts()
        {
            var result = await _newsPostService.GetAllNewsPosts();

            return result;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getNewsPost")]
        public async Task<ActionResult<Post>> GetNewsPost(Guid id)
        {
            var result = await _newsPostService.GetNewsPostById(id);
            return result;
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Editor")]
        [Route("editNewsPost")]
        public async Task<IActionResult> EditNewsPost(Guid id, NewsPostCommandRequest command)
        {
            var result = await _newsPostService.EditNewsPost(id, command);
            
            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        //[Authorize(Roles = "Admin, Editor")]
        [Route("createNewsPost")]
        public async Task<ActionResult<Post>> CreateNewsPost(NewsPostCommandRequest command)
        {

            var result = await _newsPostService.CreateNewsPost(command);

            return Ok(result);

        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("deleteNewsPost")]
        public async Task<IActionResult> DeleteNewsPost(Guid id)
        {

            var result = await _newsPostService.DeleteNewsPost(id);

            return result;
        }

        [HttpGet]
        [AllowAnonymous]

        //[Authorize(Roles = "Admin, Editor")]
        [Route("getNewsFromApi")]
        public async Task<IActionResult> GetNewsFromApi()
        {
            var result = await _newsPostService.GetNewsFromApi();

            return result;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Editor")]
        [Route("getFetchedNews")]
        public async Task<ActionResult<IEnumerable<ApiPost>>> GetFetchedNews()
        {
            var result = await _newsPostService.GetFetchedNews();

            return result;
        }
    }
}
