using Blazored.LocalStorage;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Rewriting.WebApp;

public class BearerTokenService : IAuthService
{
    private const string AccessTokenKey = "accessToken";
    private const string RefreshTokenKey = "refreshToken";

    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;
    private readonly WebAppSettings _settings;

    public BearerTokenService(ILocalStorageService localStorage, HttpClient httpClient, WebAppSettings settings)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
        _settings = settings;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var jwt = await _localStorage.GetItemAsync<TokenModel>(AccessTokenKey);

        if (jwt is null)
            return "";

        if (jwt.ExpireDate < DateTime.UtcNow)
            return jwt.Token;

        var refreshToken = await _localStorage.GetItemAsync<string>(RefreshTokenKey);

        var accessToken = await RefreshAccessTokenAsync(refreshToken);

        await _localStorage.SetItemAsync(AccessTokenKey, accessToken);

        return accessToken.Token;
    }

    public async Task<LoginResult> LoginAsync(LoginModel loginModel)
    {
        var requestBody = new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", _settings.Client),
            new KeyValuePair<string, string>("client_secret", _settings.Secret),
            new KeyValuePair<string, string>("username", loginModel.Email),
            new KeyValuePair<string, string>("password", loginModel.Password)
        };

        var loginResult = await RequestAsync(requestBody);

        var accessToken = new TokenModel(loginResult.AccessToken, loginResult.ExpiresIn);

        await _localStorage.SetItemAsync(AccessTokenKey, accessToken);
        await _localStorage.SetItemAsync(RefreshTokenKey, loginResult.RefreshToken);

        return loginResult;
    }

    private async Task<LoginResult> RequestAsync(KeyValuePair<string, string>[] requestBody)
    {
        var url = _settings.IdentityTokenUri;

        var requestContent = new FormUrlEncodedContent(requestBody);
        var response = await _httpClient.PostAsync(url, requestContent);
        var content = await response.Content.ReadAsStringAsync();

        var loginResult = JsonSerializer.Deserialize<LoginResult>(content, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new LoginResult();
        loginResult.Successful = response.IsSuccessStatusCode;

        return loginResult;
    }

    private async Task<TokenModel> RefreshAccessTokenAsync(string refreshToken)
    {
        var requestBody = new[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("client_id", _settings.Client),
            new KeyValuePair<string, string>("client_secret", _settings.Secret),
            new KeyValuePair<string, string>("refresh_token", refreshToken),
        };

        var loginResult = await RequestAsync(requestBody);

        var accessToken = new TokenModel(loginResult.AccessToken, loginResult.ExpiresIn);

        await _localStorage.SetItemAsync(RefreshTokenKey, loginResult.RefreshToken);

        return accessToken;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(AccessTokenKey);
        await _localStorage.RemoveItemAsync(RefreshTokenKey);
    }
}
