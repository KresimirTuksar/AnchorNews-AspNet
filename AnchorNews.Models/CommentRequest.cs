using System.ComponentModel.DataAnnotations;

namespace AnchorNews.Models
{
    public class CommentRequest
    {
        [Required]
        public Guid NewsPostId { get; set; }
        [Required]
        public string CommentText { get; set; }

    }
}
