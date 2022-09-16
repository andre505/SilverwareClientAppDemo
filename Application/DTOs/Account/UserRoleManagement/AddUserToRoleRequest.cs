using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Account.UserRoleManagement
{
    public class AddUserToRolesRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public IEnumerable<RolestoAdd> Roles { get; set; }
    }

    public class RolestoAdd
    {
        [Required]
        public string RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public bool Selected { get; set; }
    }


}
