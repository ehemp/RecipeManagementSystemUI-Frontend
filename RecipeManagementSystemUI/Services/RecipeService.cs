using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Diagnostics.Metrics;
using Newtonsoft.Json;
using RecipeManagementSystemUI.Models;
using System.Collections;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;


public class RecipeService
{
    private readonly HttpClient _httpClient;

    public RecipeService(IHttpClientFactory httpClientFactory)
    {
        //_httpClient = httpClient;
        _httpClient = httpClientFactory.CreateClient("AuthenticatedClient");
    }

    public async Task<List<Recipe>> GetAllRecipesAsync()
    {
        var response = await _httpClient.GetAsync("https://localhost:7256/api/Recipes");
        //var response2 = await _httpClient.GetAsync("https://localhost:7256/api/Recipes/xml");
        var content = await response.Content.ReadAsStringAsync();
        //var content2 = await response2.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        //var content = await response.Content.ReadAsStringAsync();
        //Console.WriteLine($"Content 2: {JsonConvert.DeserializeObject<List<Recipe>>(content2)[0].DynamicFields[]}");
        var allContent = content;// + content2;
        return JsonConvert.DeserializeObject<List<Recipe>>(allContent);
    }

    public async Task<string> GetAllXMLRecipesAsync()
    {
        var response = await _httpClient.GetAsync("https://localhost:7256/api/Recipes/xml");
        var content = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        //var content = await response.Content.ReadAsStringAsync();
        foreach (var item in JsonConvert.DeserializeObject<List<Recipe>>(content))
        {
            Console.WriteLine(item);
        }
        //Console.WriteLine($"Xml Content: {content}");
        //return JsonConvert.DeserializeObject<List<Recipe>>(content);
        return content;
    }

    public async Task AddRecipeAsync(Recipe recipe)
    {
        recipe.RecipeId = Guid.NewGuid().ToString();
        recipe.LastUpdated = DateTime.Now;
        Console.WriteLine(recipe.Name, recipe.LastUpdated);
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7256/api/Recipes/add", recipe);
        response.EnsureSuccessStatusCode();

    }
    public async Task UpdateRecipeAsync(Recipe recipe)
    {
        Console.WriteLine(JsonConvert.SerializeObject(recipe));
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:7256/api/Recipes/update/{recipe.RecipeId}", recipe);
        response.EnsureSuccessStatusCode();
    }
    public async Task<Recipe> GetRecipeByIdAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<Recipe>($"https://localhost:7256/api/Recipes/{id}");
    }

    public async Task<Recipe> DeleteRecipeAsync(string id)
    {   
        return await _httpClient.DeleteFromJsonAsync<Recipe>($"https://localhost:7256/api/Recipes/delete/{id}");
    }
}
