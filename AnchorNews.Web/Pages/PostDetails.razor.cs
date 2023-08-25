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
        CommentsService commentsService { get; set; }
        [Parameter]
        public string PostId { get; set; }

        public Post Post { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        public bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            commentsService.CommentAdded += RefreshComments;
            Post = await AnchorNewsService.GetPostDetails(PostId);
            RefreshComments();
        }

        protected async void RefreshComments()
        {
            isLoading = true;
            Comments = await commentsService.GetCommentsAsync(PostId);
            isLoading = false;
            StateHasChanged();
        }
    }
}
