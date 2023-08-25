using AnchorNews.Models;
using AnchorNews.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel.DataAnnotations;

namespace AnchorNews.Web.Components
{
    partial class AddCommentComponent
    {
        [Inject]
        CommentsService commentsService { get; set; }

        [Parameter]
        public string PostId { get; set; }

        public CommentRequest commentRequest { get; set; } = new CommentRequest() { NewsPostId = Guid.NewGuid() };
        string error { get; set; }

        protected async void OnSubmit()
        {
            error = null;

            commentRequest.NewsPostId = Guid.Parse(PostId);
            try
            {
                await commentsService.AddComment(commentRequest);
                commentRequest = new();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                StateHasChanged();
            }

        }

    }

}
