namespace AnchorNews_AspNet.Models.ApiNewsPost
{
    public class ApiNewsResponse
    {
        public IEnumerable<FetchedNews> Articles { get; set; }
    }
    public class FetchedNews
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
