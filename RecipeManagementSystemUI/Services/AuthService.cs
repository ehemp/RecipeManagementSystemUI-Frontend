using System.Net.Http;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using System.Threading.Tasks;
using System;
using RecipeManagementSystemUI.Models;
using Microsoft.Extensions.Diagnostics.Metrics;
using System.Net;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RecipeManagementSystemUI.Component;
using Microsoft.AspNetCore.Components.Authorization;
using System.Data;
using Microsoft.AspNetCore.Components.Routing;

public class AuthService : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly UserStateService _userStateService;
    private readonly AuthGuard authGuard;
    public bool Authenticated { get; private set; }
    private readonly LocalUserInfo localUserInfo = new LocalUserInfo();
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());
    private const string TOKEN_KEY = "authToken";
    public event Action? OnAuthStateChanged;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage, UserStateService userStateService)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _userStateService = userStateService;
        authGuard = new();
    }

    public void NotifyAuthStateChanged()
    {
        OnAuthStateChanged?.Invoke();
        //Console.WriteLine($"NotifyAuthStateChanged Length: {OnAuthStateChanged.GetInvocationList().Length}");
    }

    public async Task<bool> GetUserAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(TOKEN_KEY);
        if (string.IsNullOrEmpty(token))
        {
            new AuthenticationState(_currentUser);
            return false;
        }
        
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        //var username = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        //var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        localUserInfo.Username = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        localUserInfo.Role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        bool tokenPresent = !string.IsNullOrEmpty(token);
        bool authorized = await IsTokenExpiredAsync();
        if (await GetAuthenticationStateAsync() != null && authorized)
        {
            Console.WriteLine($"GetUserAsync: {_currentUser.Identity.Name} {_currentUser.Identity.IsAuthenticated}");
            bool? isAdmin = localUserInfo.Role == "Admin";
            _userStateService.SetUser(localUserInfo.Role ?? UserInfo.guest, localUserInfo.Username ?? UserInfo.guest, isAdmin ?? false, tokenPresent);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
            NotifyAuthStateChanged();
            Authenticated = authorized;
            return authorized;//new LoginResponse { Username = username, Role = role };
        }
        else
        {
            return false;
        }
        
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        LoginResponse loginResponse = new LoginResponse();
        var token = await _localStorage.GetItemAsync<string>(TOKEN_KEY);
        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(_currentUser);
        }
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, localUserInfo.Username),
            new Claim(ClaimTypes.Role, localUserInfo.Role)
        };
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        Console.WriteLine($"GetAuthenticationStateAsync: {_currentUser.Identity.Name}");
        loginResponse.Authorized = !string.IsNullOrEmpty(token);
        return new AuthenticationState(_currentUser);
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7256/api/Login", new { username, password });

        //var GetUserInfo = await _localStorage.GetItemAsync<LoginResponse>("user");
        LoginResponse loginResponse = new LoginResponse();
        try
        {
            Console.WriteLine($"IsSuccessStatusCode: {response.IsSuccessStatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                return loginResponse.Authorized;
            }

            try
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                
                if (result != null)
                {
                    await _localStorage.SetItemAsync("authToken", result.Token);
                    await _localStorage.SetItemAsync("tokenExpires", result.Expires);
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(result.Token);
                    localUserInfo.Username = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    localUserInfo.Role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                    loginResponse.Authorized = !string.IsNullOrEmpty(result.Token);
                    bool? isAdmin = localUserInfo.Role == "Admin";
                    // Update the UserState
                    _userStateService.SetUser(localUserInfo.Role, localUserInfo.Username, isAdmin ?? false);
                    Console.WriteLine($"LoginAsync Authorized: {loginResponse.Authorized}");
                    if (loginResponse.Authorized)
                    {
                        NotifyAuthStateChanged();
                        await GetAuthenticationStateAsync();
                        return loginResponse.Authorized;
                    }
                    else
                    {
                        return loginResponse.Authorized;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Try Login result: " + ex.Message);
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Try Login response: " + ex.Message);
        }

        return false;
    }

    public async Task<bool> LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("tokenExpires");
        _userStateService.SetUser(UserInfo.guest, UserInfo.guest, false, false);
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        if (!_currentUser.Identity.IsAuthenticated)
        {
            NotifyAuthStateChanged();
            Console.WriteLine($"LogOutAsync AuthService {OnAuthStateChanged.GetInvocationList()[0].Target}");
            return _currentUser.Identity.IsAuthenticated;
        }
        else
        {
            Console.WriteLine($"LogoutAsync Else {_currentUser.Identity.IsAuthenticated}");
            return !_currentUser.Identity.IsAuthenticated;
        }
    }

    public async Task<bool> IsTokenExpiredAsync()
    {
        var expiration = await _localStorage.GetItemAsync<DateTime>("tokenExpires");
        Console.WriteLine($"IsTokenExpired: {expiration} {DateTime.UtcNow} {expiration <= DateTime.UtcNow}");
        //NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        return expiration > DateTime.UtcNow;
    }
}

public class LoginResponse
{
    public string Token { get; set; }
    public LocalUserInfo UserInfo {  get; set; }
    //public string Role { get; set; }
    //public string Username { get; set; }
    public bool Authorized { get; set; } = false;
    public DateTime Expires { get; set; }
}

public class LocalUserInfo
{
    public string Username { get; set; }
    public string Role { get; set; }
}
