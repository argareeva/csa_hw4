using orders.Context;
using orders.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace orders.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly DataBase _dbContext;
        private readonly IOrderProcessor _orderProcessor;

        public OrderController(DataBase dbContext, IOrderProcessor orderProcessor)
        {
            _dbContext = dbContext;
            _orderProcessor = orderProcessor;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            if (_dbContext.Order == null)
            {
                return NotFound("No orders yet");
            }

            return Ok(await _dbContext.Order.ToListAsync());
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            if (_dbContext.Order == null)
            {
                return NotFound("No orders yet");
            }
            
            var order = _dbContext.Order.FirstOrDefault(o => o.Id == id);
            
            if (order == null)
            {
                return NotFound("There is no order with this id");
            }
            return Ok(order);
        }

        // POST: api/Dish
        [HttpPost]
        public IActionResult CreateOrder(OrderDto orderDto)
        {
            if (_dbContext.Order == null)
            {
                return NotFound("No orders yet");
            }
            
            if (_dbContext.User == null)
            {
                return NotFound("No users yet");
            }
            
            var user = _dbContext.User.FirstOrDefault(t => t.Id == orderDto.UserId);
            
            if (user == null)
            {
                return NotFound("No user with this id");
            }

            var order = new Order
            {
                UserId = orderDto.UserId,
                Status = orderDto.Status,
                SpecialRequests = orderDto.SpecialRequests,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _dbContext.Order.Add(order);
            _dbContext.SaveChanges();

            if (_dbContext.Dish == null)
            {
                return NotFound("No dishes yet");
            }
            
            var dish = _dbContext.Dish.FirstOrDefault(d => d.Id == orderDto.DishId);
            
            if (dish == null)
            {
                return NotFound("There is no dish with this id");
            }
            
            var orderDish = new DishOrder
            {
                OrderId = order.Id,
                DishId = dish.Id,
                Quantity = dish.Quantity,
                Price = dish.Price
            };
            
            if (_dbContext.DishOrder == null)
            {
                return NotFound("No dish_orders yet");
            }

            _dbContext.DishOrder.Add(orderDish);
            _dbContext.SaveChanges();
            
            _orderProcessor.ProcessOrder(order);

            return Ok("Successfully created");
        }
    }
}