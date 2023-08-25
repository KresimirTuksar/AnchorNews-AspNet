using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnchorNews.Models
{
    public class AddPostRequest
    {
        [Required]
        public string Headline { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string FullDescription { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Category { get; set; }
        public bool IsBreakingNews { get; set; }
    }
}
