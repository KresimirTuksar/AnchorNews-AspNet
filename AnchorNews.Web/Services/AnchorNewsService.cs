
using AnchorNews.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AnchorNews.Web.Services
{
    public class AnchorNewsService : IAnchorNewsService
    {
        private readonly LocalStorageService _localStorageService;

        private readonly HttpClient _httpClient;
        public AnchorNewsService(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _localStorageService = localStorageService;
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Post>>("api/NewsPosts/getAllNewsPost");
        }

        public async Task<Post> GetPostDetails(string postId)
        {
            return await _httpClient.GetFromJsonAsync<Post>("api/NewsPosts/getNewsPost?id=" + postId);
        }

        public async Task AddPost(AddPostRequest AddPostRequest)
        {
            var token = await _localStorageService.GetItem<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);


            var result = await _httpClient.PostAsJsonAsync("/api/NewsPosts/createNewsPost", AddPostRequest);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var e = new Exception(await result.Content.ReadAsStringAsync());
                throw e;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var e = new UnauthorizedAccessException();
                throw e;

            }
            result.EnsureSuccessStatusCode();
        }
    }
}