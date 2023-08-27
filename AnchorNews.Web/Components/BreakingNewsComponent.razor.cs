using AnchorNews.Models;
using Microsoft.AspNetCore.Components;

namespace AnchorNews.Web.Components
{
    partial class BreakingNewsComponent
    {
        [Parameter]
        public Post BreakingNews { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }


        protected void onPostClick(Guid id)
        {
            NavigationManager.NavigateTo($"post/{id.ToString()}");
        }

    }
}
