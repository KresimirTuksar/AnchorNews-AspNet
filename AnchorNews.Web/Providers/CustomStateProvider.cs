using AnchorNews.Models;
using AnchorNews.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Data;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace AnchorNews.Web.Providers
{
    public class CustomStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService api;
        private readonly LocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public CustomStateProvider(IAuthService api, IHttpClientFactory httpClientFactory, LocalStorageService localStorage)
        {
            this.api = api;
            _localStorage =localStorage;
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userData = await _localStorage.GetItem<LoginResponse>("userData");

            if (userData is not null && userData.Expiration > DateTime.UtcNow)
            {
                var token = await _localStorage.GetItem<string>("authToken");
                List<Claim> claims = new List<Claim> {
                        new Claim(ClaimTypes.NameIdentifier, userData.UserId),
                        new Claim(ClaimTypes.Email, userData.Email),
                        new Claim(ClaimTypes.Name, userData.Username)
                    };
                foreach (var role in userData.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                return new AuthenticationState(user);
            }
            else
            {
                await _localStorage.RemoveItem("authToken");
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        public async Task Logout()
        {
            var userData = await _localStorage.GetItem<LoginResponse>("userData");

            if (userData is not null)
            {
                await _localStorage.RemoveItem("userData");
                await _localStorage.RemoveItem("authtoken");
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }
        public async Task Login(LoginRequest loginParameters)
        {
            await api.Login(loginParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task Register(RegisterRequest registerParameters)
        {
            await api.Register(registerParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
