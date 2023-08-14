using AnchorNews.Models;
using AnchorNews.Web.Services;
using Microsoft.AspNetCore.Components;

namespace AnchorNews.Web.Pages
{
    partial class TestComponent
    {
        [Inject]
        public IAnchorNewsService AnchorNewsService { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();


        protected override async Task OnInitializedAsync()
        {
            Posts = (await AnchorNewsService.GetAllPosts()).ToList();
        }
    }
}
