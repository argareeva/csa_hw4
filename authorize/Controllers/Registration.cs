using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using authorize.Tables;
using authorize.Context;

namespace authorize.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Registration : ControllerBase
    {
        private readonly DataBase _dbContext;

        public Registration(DataBase dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/registration
        [HttpPost]
        public ActionResult Register(UserRegistration userDto)
        {
            if (!IsValidEmail(userDto.Email))
            {
                return Conflict("Invalid email address");
            }

            if (userDto.Role != "manager" && userDto.Role != "customer" && userDto.Role != "chef")
            {
                return Conflict("Invalid role type");
            }

            if (_dbContext.User == null)
            {
                return NotFound("No users yet");
            }

            var existingUser = _dbContext.User.FirstOrDefault(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                return Conflict("A user with this email address is already registered");
            }

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = HashPassword(userDto.PasswordHash),
                Role = userDto.Role,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _dbContext.User.Add(user);
            _dbContext.SaveChanges();

            return Ok("Successfully registered");
        }

        // Проверка корректности адреса электронной почты
        private static bool IsValidEmail(string email)
        {
            return email.Contains('@');
        }

        // Хеширование паролей
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(passwordBytes);
            var hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }
    }
}