using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; } 
        public string MobilePhone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
         
    }
}