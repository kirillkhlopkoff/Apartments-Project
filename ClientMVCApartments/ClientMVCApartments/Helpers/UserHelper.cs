using ClientMVCApartments.Models;
using System.Text.Json;

namespace ClientMVCApartments.Helpers
{
    public class UserHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public User GetUser(string userName)
        {
            // Получение данных пользователя из куки или другого источника

            // Пример: Извлечение данных из куки с именем "UserData"
            var userDataCookie = _httpContextAccessor.HttpContext.Request.Cookies["MyCookieName"];

            // В данном примере предполагается, что данные пользователя хранятся в JSON формате в куке
            // Вы можете настроить формат хранения данных в куке в соответствии со своими потребностями

            if (!string.IsNullOrEmpty(userDataCookie))
            {
                // Декодирование JSON и создание объекта User
                var user = JsonSerializer.Deserialize<User>(userDataCookie);
                if (user.UserName == userName)
                {
                    return user;
                }
            }

            // Возвращаем null, если пользователь не найден или данные пользователя некорректны
            return null;
        }
    }
}
