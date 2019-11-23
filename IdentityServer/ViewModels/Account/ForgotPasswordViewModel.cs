using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
