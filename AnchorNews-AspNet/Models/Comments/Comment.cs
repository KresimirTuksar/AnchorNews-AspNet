using AnchorNews_AspNet.Models.NewsPost;

namespace AnchorNews_AspNet.Models.Comments
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string CommentText { get; set; }
        public string CommenterName { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid NewsPostId { get; set; }
        public Post NewsPost { get; set; }
    }
}
