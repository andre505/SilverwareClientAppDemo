using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Account
{
    public class RegisterEmployeeRequest
    {
        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }


        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number. Phone number must be 10 digits.")]
        public string PhoneNumber { get; set; }

        public string Password { get; set; }


        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
