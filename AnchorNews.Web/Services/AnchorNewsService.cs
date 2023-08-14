
using AnchorNews.Models;
using System.Net.Http;

namespace AnchorNews.Web.Services
{
    public class AnchorNewsService : IAnchorNewsService
    {
        private readonly HttpClient _httpClient;
        //public AnchorNewsService(HttpClient httpClient)
        //{
        //    _httpClient=httpClient;
        //}
        public AnchorNewsService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Post>>("api/NewsPosts/getAllNewsPost");
        }
    }
}