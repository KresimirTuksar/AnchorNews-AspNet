using AnchorNews.Models;
using AnchorNews.Web.Helpers;
using Microsoft.AspNetCore.Components;

namespace AnchorNews.Web.Components
{
    partial class CommentComponent
    {
        [Parameter]
        public Comment Comment { get; set; }

        public string humanizedTime;

        protected override void OnInitialized()
        {
            humanizedTime = HelpersGeneral.HumanizeTime(Comment.Timestamp);
        }
    }
}
