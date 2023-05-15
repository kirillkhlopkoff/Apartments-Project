using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiApartmentIdentity
{
    public class AuthOptions
    {
        public const string ISSUER = "ApiApartmentIdentity"; // издатель токена
        public const string AUDIENCE = "ApiApartment"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!1234567890123456";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
