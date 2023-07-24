using System.ComponentModel.DataAnnotations;

namespace Api.ViewModels.Account
{
    public class LoginView
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
