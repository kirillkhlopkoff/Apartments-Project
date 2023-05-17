using System.ComponentModel.DataAnnotations;

namespace ClientMVCApartments.ViewModels.Account
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
