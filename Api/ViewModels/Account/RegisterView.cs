using System.ComponentModel.DataAnnotations;

namespace Api.ViewModels.Account
{
    public class RegisterView
    {
        [Required]
        [StringLength(15,MinimumLength = 3,ErrorMessage = "FirstName must be atleast {2} and maximum {1} characters")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "LastName must be atleast {2} and maximum {1} characters")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }   // Also UserName
        [Required]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Password must be atleast {2} and maximum {1} characters")]
        public string Password { get; set; }
    }
}
