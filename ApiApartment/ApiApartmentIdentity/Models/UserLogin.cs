using System.ComponentModel.DataAnnotations;

namespace ApiApartmentIdentity.Models
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
