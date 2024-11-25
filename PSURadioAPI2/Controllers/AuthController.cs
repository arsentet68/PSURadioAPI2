using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSURadioAPI2.Models;

namespace PSURadioAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // POST api/auth
        [HttpPost]
        public async Task<ActionResult<Auth>> Post(Auth auth)
        {
            if (auth == null)
            {
                return BadRequest();
            }

            // Здесь вы можете добавить логику для аутентификации пользователя,
            // например, проверку логина и пароля в базе данных.
            // Для простоты мы просто возвращаем полученные данные.

            // Пример проверки (упрощенной) можно добавить сюда.
            AuthResult authResult = ValidateUser(auth.Login, auth.Password);

            if (authResult == null)
            {
                return Unauthorized();
            }

            return Ok(authResult);
        }

        public AuthResult ValidateUser(string usernameOrEmail, string password)
        {
            using (var dbContext = new PsuradioContext())
            {
                // Поиск пользователя по логину или email в таблице Users
                var user = dbContext.Users.FirstOrDefault(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);

                // Если пользователь с таким логином или email найден, проверяем пароль
                if (user != null && password == user.Password)
                {
                    return new AuthResult { Id = user.Id, Username = user.Username, Role = user.Role, Email = user.Email, ProfilePic = user.ProfilePic};
                }
            }

            // Если не найдено совпадение ни по логину, ни по email, возвращаем false
            return null;
        }
    }
}
