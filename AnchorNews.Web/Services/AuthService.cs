using AnchorNews.Models;
using AnchorNews.Web.Pages;
using AnchorNews.Web.Services;
using System.Text.Json;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly LocalStorageService _localstorage;

    public AuthService(IHttpClientFactory httpClientFactory, LocalStorageService localstorage)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _localstorage=localstorage;
    }


    public async Task Login(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", loginRequest);

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();
        var authResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        await _localstorage.SetItem("authToken", authResponse.Token);
        await _localstorage.SetItem("userData", authResponse);
    }

    public async Task Logout()
    {
        var result = await _httpClient.PostAsync("auth/logout", null);
        result.EnsureSuccessStatusCode();
    }

    public async Task Register(RegisterRequest registerRequest)
    {
        var result = await _httpClient.PostAsJsonAsync("auth/register", registerRequest);
        if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
        result.EnsureSuccessStatusCode();
    }

}