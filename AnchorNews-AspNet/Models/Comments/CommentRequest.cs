namespace AnchorNews_AspNet.Models.Comments
{
    public class CommentRequest
    {
        public Guid NewsPostId { get; set; }

        public string CommentText { get; set; }

    }
}
