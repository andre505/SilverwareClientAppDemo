using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Account.RoleManagement
{
    public class CreateRoleRequest
    {
        [Required]
        public string role { get; set; }
    }
}
