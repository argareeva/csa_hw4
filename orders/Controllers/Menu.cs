using orders.Context;
using orders.Tables;
using Microsoft.AspNetCore.Mvc;

namespace orders.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Menu : ControllerBase
    {
        private readonly DataBase _dbContext;

        public Menu(DataBase dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Menu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetMenu()
        {
            if (_dbContext.Dish == null)
            {
                return NotFound("No dishes yet");
            }

            var dishes = new List<Dish>();

            foreach (var dish in _dbContext.Dish)
            {
                if (dish.IsAvailable)
                {
                    dishes.Add(dish);
                }
            }

            return Ok(dishes);
        }
    }
}