using AnchorNews.Models;
using System.Net.Http.Headers;

namespace AnchorNews.Web.Services
{
    public class CommentsService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageService _localStorageService;

        public Action CommentAdded { get; set; }
        public CommentsService(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }


        public async Task<IEnumerable<Comment>> GetCommentsAsync(string postId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Comment>>("api/comments?postId=" + postId);
        }

        public async Task AddComment(CommentRequest commentRequest)
        {
            var token = await _localStorageService.GetItem<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);


            var result = await _httpClient.PostAsJsonAsync("api/comments", commentRequest);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) 
            {
                var e = new Exception(await result.Content.ReadAsStringAsync());
                throw e;
            }
            else if(result.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                var e = new UnauthorizedAccessException();
                throw e;

            }
            PublishEvent();
            result.EnsureSuccessStatusCode();
        }

        public void PublishEvent()
        {
            CommentAdded?.Invoke();
        }
    }
}
