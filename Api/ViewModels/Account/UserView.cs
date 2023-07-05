using System.ComponentModel.DataAnnotations;

namespace Api.ViewModels.Account
{
    public class UserView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JWT { get; set; }
    }
}
