using BookStore.Model;
using BookStore.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Order> OrderRepo;

        public OrderController(IRepository<Order> _orderRepo)
        {
            OrderRepo = _orderRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Order> orders = (await OrderRepo.GetAllAsync()).ToList();
           
            return Ok(orders);
        }
    }
}
