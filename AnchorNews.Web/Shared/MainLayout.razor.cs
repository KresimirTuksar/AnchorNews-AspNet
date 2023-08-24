using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using AnchorNews.Web.Providers;

namespace AnchorNews.Web.Shared
{
    partial class MainLayout
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }
        [Inject]
        public CustomStateProvider authStateProvider{ get; set; }
        [CascadingParameter]
        public AuthenticationState AuthenticationState { get; set; }

        async Task LogoutClick()
        {
            await authStateProvider.Logout();
            navigationManager.NavigateTo("");
        }

        async Task LoginClick()
        {
            navigationManager.NavigateTo("/login");
        }
        async Task RegisterClick()
        {
            navigationManager.NavigateTo("/register");
        }

    }
}

