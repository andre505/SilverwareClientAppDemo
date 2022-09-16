using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Account.UserRoleManagement
{
    public class GetStaffWithRolesRequest
    {

        [Required]
        public string userId { get; set; }
    }
}
