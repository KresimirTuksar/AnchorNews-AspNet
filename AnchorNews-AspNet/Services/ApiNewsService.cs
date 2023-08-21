using AnchorNews_AspNet.Models.ApiNewsPost;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Newtonsoft.Json;

public class NewsApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public NewsApiService(HttpClient httpClient)
    {
        //_httpClient = new HttpClient();
        _httpClient = httpClient;
        _apiKey = "c0b69e88ffda4fdba9b00d5136cfc827";
    }

    public async Task<ArticlesResult> FetchNewsPostsAsync()
    {
        var newsApiClient = new NewsApiClient("089b883722224bcb8c396f3784631fb2");
        var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
        {
            Q = "Apple",
            SortBy = SortBys.Popularity,
            Language = Languages.EN,
            From = new DateTime(2023, 7, 25)
        });
        //var url = "https://newsapi.org/v2/everything?q=tesla&from=2023-07-16&sortBy=publishedAt&apiKey=089b883722224bcb8c396f3784631fb2";

        //var response = await _httpClient.GetAsync(url);

        //if (response.IsSuccessStatusCode)
        //{
        //    var json = await response.Content.ReadAsStringAsync();
        //    var apiResponse = JsonConvert.DeserializeObject<ApiNewsResponse>(json);

        //    return articlesResponse;
        //}

        //// Handle error scenarios

        return articlesResponse;
    }
}