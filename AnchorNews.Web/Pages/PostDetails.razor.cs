using AnchorNews.Models;
using AnchorNews.Web.Services;
using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata;

namespace AnchorNews.Web.Pages
{
    partial class PostDetails
    {
        [Inject]
        IAnchorNewsService AnchorNewsService { get; set; }
        [Inject]
        ICommentsService CommentsService { get; set; }
        [Parameter]
        public string PostId { get; set; }

        public Post Post { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Post = await AnchorNewsService.GetPostDetails(PostId);
            Comments = await CommentsService.GetCommentsAsync(PostId);
        }
    }
}
