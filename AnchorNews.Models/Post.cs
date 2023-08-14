namespace AnchorNews.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Headline { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public bool IsBreakingNews { get; set; }
        public DateTime? BreakingNewsExpiration { get; set; }
        public int ViewCount { get; set; }

        public List<Comment> Comments { get; set; }

    }
}