using orders.Context;
using orders.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace orders.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly DataBase _dbContext;

        public DishController(DataBase dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Dish
        [HttpGet]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
        {
            if (!User.IsInRole("manager"))
            {
                return Forbid("Only managers are allowed");
            }
            
            if (_dbContext.Dish == null)
            {
                return NotFound("No dishes yet");
            }

            return Ok(await _dbContext.Dish.ToListAsync());
        }

        // GET: api/Dish/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "manager")]
        public IActionResult GetDishById(int id)
        {
            if (!User.IsInRole("manager"))
            {
                return Forbid("Only managers are allowed");
            }
            
            if (_dbContext.Dish == null)
            {
                return NotFound("No dishes yet");
            }
            
            var dish = _dbContext.Dish.FirstOrDefault(d => d.Id == id);
            if (dish == null)
            {
                return NotFound("There is no dish with this id");
            }
            
            return Ok(dish);
        }

        // POST: api/Dish
        [HttpPost]
        [Authorize(Roles = "manager")]
        public IActionResult CreateDish(DishDto dishDto)
        {
            if (!User.IsInRole("manager"))
            {
                return Forbid("Only managers are allowed");
            }
            
            if (_dbContext.Dish == null)
            {
                return NotFound("No dishes yet");
            }

            var dish = new Dish
            {
                Name = dishDto.Name,
                Description = dishDto.Description,
                Price = dishDto.Price,
                Quantity = dishDto.Quantity,
                IsAvailable = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            
            if (dishDto.Quantity == 0)
            {
                dish.IsAvailable = false;
            }
            
            _dbContext.Dish.Add(dish);
            _dbContext.SaveChanges();
            
            return Ok("Successfully created");
        }

        // PUT: api/Dish/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "manager")]
        public IActionResult UpdateDish(int id, DishDto dishDto)
        {
            if (!User.IsInRole("manager"))
            {
                return Forbid("Only managers are allowed");
            }
            
            if (_dbContext.Dish == null)
            {
                return NotFound("No dishes yet");
            }
            
            var dish = _dbContext.Dish.FirstOrDefault(d => d.Id == id);
            if (dish == null)
            {
                return NotFound("There is no dish with this id");
            }

            dish.Name = dishDto.Name;
            dish.Description = dishDto.Description;
            dish.Price = dishDto.Price;
            dish.Quantity = dishDto.Quantity;
            dish.IsAvailable = true;
            dish.UpdatedAt = DateTime.Now;
            
            if (dish.Quantity == 0)
            {
                dish.IsAvailable = false;
            }

            _dbContext.SaveChanges();

            return Ok(dish);
        }

        // DELETE: api/Dish/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "manager")]
        public IActionResult DeleteDish(int id)
        {
            if (!User.IsInRole("manager"))
            {
                return Forbid("Only managers are allowed");
            }
            
            if (_dbContext.Dish == null)
            {
                return NotFound("No dishes yet");
            }
            
            var dish = _dbContext.Dish.FirstOrDefault(d => d.Id == id);
            if (dish == null)
            {
                return NotFound("There is no dish with this id");
            }

            _dbContext.Dish.Remove(dish);
            _dbContext.SaveChanges();

            return Ok("Successfully deleted");
        }
    }
}