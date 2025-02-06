namespace RecipeManagementSystemUI.Models
{
    public class UserInfo
    {
        public string username { get; set; }
        public string role { get; set; }
        public static string guest { get; } = "Guest";
        public bool IsAdmin { get; set; } = false;
    }
}
