using AnchorNews.Models;
using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata;

namespace AnchorNews.Web.Components
{
    partial class PostComponent
    {
        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Parameter]
        public Post Post { get; set; }

        protected void onPostClick(Guid id)
        {
            NavigationManager.NavigateTo($"post/{ id.ToString()}");
        }
    }
}
