using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using RecipeManagementSystemUI.Models;


public class AuthHttpMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly UserStateService _userStateService;
    public bool TokenPresent { get; set; } = false;
    public AuthHttpMessageHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (!string.IsNullOrEmpty(token))
        {
            Console.WriteLine($"HTTP Message handler: {token}");
            // Update the UserState
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //bool isAdmin = GetUserInfo.role == "Admin";
            //_userStateService?.SetUser(GetUserInfo.role, GetUserInfo.username, isAdmin, TokenPresent);
        }

        return await base.SendAsync(request, cancellationToken);
    }

}
