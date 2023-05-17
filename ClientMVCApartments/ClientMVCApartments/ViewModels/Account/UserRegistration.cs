using System.ComponentModel.DataAnnotations;

namespace ClientMVCApartments.ViewModels.Account
{
    public class UserRegistration
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
