using System.ComponentModel.DataAnnotations;

namespace RecipeManagementSystemUI.Models
{
    public class User
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
        [StringLength(100)]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }
        public string UserId { get; set; }
    }
}
