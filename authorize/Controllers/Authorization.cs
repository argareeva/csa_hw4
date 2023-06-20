using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using authorize.Tables;
using authorize.Context;
using Microsoft.IdentityModel.Tokens;

namespace authorize.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Authorization : ControllerBase
    {
        private readonly DataBase _dbContext;
        private readonly IConfiguration _configuration;

        public Authorization(IConfiguration configuration, DataBase dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }
        
        // GET: api/Authorization
        [HttpPost]
        public IActionResult Login(UserLogin user)
        {
            if (_dbContext.User == null)
            {
                return NotFound("No users yet");
            }
            
            var existingUser = _dbContext.User.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser == null || existingUser.PasswordHash != HashPassword(user.PasswordHash) ||
                                      existingUser.Email != user.Email)
            {
                return Unauthorized("Invalid password or email address");
            }

            var token = GenerateToken(existingUser);

            var session = new Session
            {
                UserId = existingUser.Id,
                SessionToken = token,
                ExpiresAt = DateTime.Now.AddDays(1)
            };
            
            if (_dbContext.Session == null)
            {
                return NotFound("No sessions yet");
            }

            _dbContext.Session.Add(session);
            _dbContext.SaveChanges();
            
            return Ok(new { Token = token });
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
        
        // Генерация токена
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}