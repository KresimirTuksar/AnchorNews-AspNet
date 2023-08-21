using AnchorNews.Models;

namespace AnchorNews.Web.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly HttpClient _httpClient;
        public CommentsService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }


        public async Task<IEnumerable<Comment>> GetCommentsAsync(string postId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Comment>>("api/comments?postId=" + postId);
        }
    }
}
