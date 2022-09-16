using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Account
{
    public class CreateCustomClaimsModel
    {
        [Required]
        public string ClaimType { get; set; }
        [Required]
        public string ClaimValue { get; set; }

    }
}
