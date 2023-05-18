using AnchorNews_AspNet.Models.ApiNewsPost;
using Newtonsoft.Json;

public class NewsApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public NewsApiService()
    {
        _httpClient = new HttpClient();
        _apiKey = "c0b69e88ffda4fdba9b00d5136cfc827";
    }

    public async Task<IEnumerable<FetchedNews>> FetchNewsPostsAsync()
    {
        var url = "https://newsapi.org/v2/top-headlines?country=us&apiKey=c0b69e88ffda4fdba9b00d5136cfc827";

        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiNewsResponse>(json);

            return apiResponse.Articles;
        }

        // Handle error scenarios

        return Enumerable.Empty<FetchedNews>();
    }
}