using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using PSURadioAPI2.Models; // Поменяйте на актуальное пространство имен вашего проекта

namespace PSURadioAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly PsuradioContext _context;

        public UsersController(PsuradioContext context)
        {
            _context = context;
        }

        [HttpPut("{id}/username")]
        public async Task<IActionResult> UpdateUserName(int id, [FromBody] UpdateUserNameRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName))
            {
                return BadRequest("Имя пользователя не может быть пустым.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            user.Username = request.UserName;
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound("Пользователь не найден.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateUserRoleRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            user.Role = request.Role;
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound("Пользователь не найден.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersByRole(int role)
        {
            var users = await _context.Users.Where(u => u.Role == role).ToListAsync();
            return Ok(users);
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] UserRegistrationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Все поля обязательны для заполнения.");
            }

            if (await _context.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email))
            {
                return Conflict("Имя пользователя или email уже заняты.");
            }

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password, // Consider hashing the password before saving it
                Role = 1,
                ProfilePic = null
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }

    public class UpdateUserNameRequest
    {
        public string UserName { get; set; }
    }
    public class UpdateUserRoleRequest
    {
        public int Role { get; set; }
    }
    public class UserRegistrationRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

