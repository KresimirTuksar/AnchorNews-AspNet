using AnchorNews.Models;
using AnchorNews.Web.Services;
using Microsoft.AspNetCore.Components;

namespace AnchorNews.Web.Pages
{
    partial class Posts

    {
        [Inject]
        public IAnchorNewsService AnchorNewsService { get; set; }

        public List<Post> PostsList { get; set; } = new List<Post>();


        protected override async Task OnInitializedAsync()
        {
            PostsList = (await AnchorNewsService.GetAllPosts()).ToList();
        }
    }
}
