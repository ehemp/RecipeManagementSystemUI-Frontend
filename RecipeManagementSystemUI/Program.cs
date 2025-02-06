using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RecipeManagementSystemUI;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Toolbelt.Blazor;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Reflection.Emit;

string backend = "https://localhost:7256/";
string frontend = "https://localhost:7101/";
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient for the WebAssembly environment
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Optionally, register other services or custom HTTP clients as needed
builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = backend;
    options.ProviderOptions.ClientId = frontend;
});
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(backend) });

builder.Services.AddScoped<AuthHttpMessageHandler>();
builder.Services.AddHttpClient("AuthenticatedClient", client =>
{
    client.BaseAddress = new Uri(backend);
}).AddHttpMessageHandler<AuthHttpMessageHandler>();

/*builder.Services.AddScoped(async sp =>
{
    var client = new HttpClient { BaseAddress = new Uri(backend) };
    Console.WriteLine("TEST!!! " + client);
    var localStorageService = sp.GetService<ILocalStorageService>();
    if (localStorageService != null)
    {
        var authToken = await localStorageService.GetItemAsync<string>("authToken");
        Console.WriteLine("authToken: " + authToken);
        if (!string.IsNullOrEmpty(authToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }
    }
    Console.WriteLine($"Client: {client}");
    return client;
});*/


builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<UserStateService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpClientInterceptor();
builder.Services.AddScoped<HttpInterceptorService>();
builder.Services.AddScoped<Newtonsoft.Json.JsonSerializer>();




await builder.Build().RunAsync();
