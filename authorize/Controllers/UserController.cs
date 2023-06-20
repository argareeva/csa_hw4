using Microsoft.AspNetCore.Mvc;
using authorize.Tables;
using authorize.Context;
using Microsoft.EntityFrameworkCore;

namespace authorize.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataBase _dbContext;

        public UserController(DataBase dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            if (_dbContext.User == null)
            {
                return NotFound("No users yet");
            }

            return Ok(await _dbContext.User.ToListAsync());
        }
        
        // GET: api/User/token
        [HttpGet("{token}")]
        public async Task<ActionResult<User>> GetUserByToken(string token)
        {
            if (_dbContext.Session == null)
            {
                return NotFound("No sessions yet");
            }

            var userToken = _dbContext.Session.FirstOrDefault(t => t.SessionToken == token);

            if (userToken == null)
            {
                return NotFound("No user with this token");
            }

            if (_dbContext.User == null)
            {
                return NotFound("No users yet");
            }

            var user = await _dbContext.User.FindAsync(userToken.UserId);

            return user;
        }
    }
}