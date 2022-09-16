using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Account
{
    public class AddToRoleModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
