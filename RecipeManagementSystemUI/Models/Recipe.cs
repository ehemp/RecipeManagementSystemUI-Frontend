namespace RecipeManagementSystemUI.Models
{
    public class Recipe
    {
        public string RecipeId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Tool { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public DateTime LastUpdated { get; set; }
        public Dictionary<string, object>? DynamicFields { get; set; } = new();
    }
}

