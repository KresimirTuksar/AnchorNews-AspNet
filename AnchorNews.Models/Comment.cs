using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnchorNews.Models
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
