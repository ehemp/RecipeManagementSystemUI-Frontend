using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net;
using Toolbelt.Blazor;

public class HttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly NavigationManager _navigation;
    private readonly ILocalStorageService _localStorage;

    public HttpInterceptorService(HttpClientInterceptor interceptor, NavigationManager navigation, ILocalStorageService localStorage)
    {
        _interceptor = interceptor;
        _navigation = navigation;
        _localStorage = localStorage;
    }

    public void RegisterEvent() => _interceptor.AfterSendAsync += InterceptResponse;

    private async Task InterceptResponse(object sender, HttpClientInterceptorEventArgs e)
    {
        if (e.Response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _localStorage.RemoveItemAsync("authToken");
            _navigation.NavigateTo("/login");
        }
    }
}
