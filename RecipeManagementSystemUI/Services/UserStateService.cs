using Blazored.LocalStorage;
using Newtonsoft.Json;
using RecipeManagementSystemUI.Models;

public class UserStateService
{
    public string UserRole { get; private set; } = UserInfo.guest;
    public string UserName { get; private set; } = UserInfo.guest;
    public bool IsAdmin { get; private set; }
    public bool TokenPresent { get; set; }
    

    public event Action? OnChange;
    //private readonly UserInfo userInfo;

    public void SetUser(string? role, string? username, bool isAdmin)
    {
        UserRole = role ?? UserInfo.guest;
        UserName = username ?? UserInfo.guest;
        IsAdmin = isAdmin;
        NotifyStateChanged();
    }
    public void SetUser(string? role, string? username, bool isAdmin, bool tokenPresent)
    {
        UserRole = role ?? UserInfo.guest;
        UserName = username ?? UserInfo.guest;
        IsAdmin = isAdmin;
        TokenPresent = tokenPresent;
        Console.WriteLine($"UserStateService TokenPresent: {TokenPresent}");
        NotifyStateChanged();
    }
    private void NotifyStateChanged() => OnChange?.Invoke();
}
