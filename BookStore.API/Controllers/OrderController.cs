using BookStore.DTO.OrderDTO;
using BookStore.Model;
using BookStore.Repository;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Order> OrderRepo;
        private readonly IRepository<Book> BookRepo;
        private readonly UnitOfWorks UnitOfWork;
        private readonly IRepository<OrderItems> OrderItemRepo;

        public OrderController(IRepository<Order> _orderRepo, IRepository<Book> _bookRepo, UnitOfWorks _unitOfWork, IRepository<OrderItems> _orderItemRepo)
        {
            OrderRepo = _orderRepo;
            BookRepo = _bookRepo;
            UnitOfWork = _unitOfWork;
            OrderItemRepo = _orderItemRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            List<Order> orders = (await OrderRepo.GetAllAsync(Include: "OrderItems.Book")).ToList();

            if (orders == null || orders.Count == 0)
                return NotFound("No orders found");

            List<GetOrderDTO> getOrderDTOs = new List<GetOrderDTO>();
            foreach (var order in orders)
            {
                GetOrderDTO getOrderDTO = new GetOrderDTO()
                {
                    Id = order.Id,
                    CustomerName = order.CustomerName,
                    TotalPrice = order.TotalPrice,
                    createsDTO = order.OrderItems.Select(item => new CreateOrderItemDTO
                    {
                        BookTitle = item.Book?.Title ?? "Unknown",
                        Quantity = item.Quantity
                    }).ToList()
                };
                getOrderDTOs.Add(getOrderDTO);
            }
            return Ok(getOrderDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrederById(int id)
        {

            Order order = await OrderRepo.Get(o => o.Id == id, Include: "OrderItems.Book");

            if (order == null)
                return NotFound("Order not found");

            GetOrderDTO getOrderDTO = new GetOrderDTO()
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                TotalPrice = order.TotalPrice,
                createsDTO = order.OrderItems.Select(item => new CreateOrderItemDTO
                {
                    BookTitle = item.Book?.Title ?? "Unknown",
                    Quantity = item.Quantity
                }).ToList()
            };
            return Ok(getOrderDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO CreateOrderdto)
        {
            Order order = new Order
            {
                CustomerName = CreateOrderdto.CustomerName,
                OrderItems = new List<OrderItems>()
            };

            int totalPrice = 0;

            foreach (var item in CreateOrderdto.createsDTO)
            {
                var book = await BookRepo.Get(o => o.Title == item.BookTitle);

                if (book == null)
                    return NotFound($"Book with Title {item.BookTitle} not found");

                if (item.Quantity > book.Quantity)
                    return BadRequest($"Quantity for book {item.BookTitle} exceeds available stock");

                order.OrderItems.Add(new OrderItems
                {
                    BookId = book.Id,
                    Quantity = item.Quantity
                });
                book.Quantity -= item.Quantity; // Reduce the quantity of the book in stock
                totalPrice += book.Price * item.Quantity;
            }

            order.TotalPrice = totalPrice;

            await OrderRepo.Add(order);
            UnitOfWork.SaveAndCommit();


            GetOrderDTO getOrderDTO = new GetOrderDTO
            {
                CustomerName = order.CustomerName,
                TotalPrice = order.TotalPrice,
                createsDTO = CreateOrderdto.createsDTO
            };
            return Ok(getOrderDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderDTO updateOrderDTO)
        {
            if (ModelState.IsValid)
            {
                Order order = await OrderRepo.Get(o => o.Id == id, Include: "OrderItems.Book");
                if (order == null)
                {
                    return NotFound($"Order with Id {id} not found");
                }

                order.CustomerName = updateOrderDTO.CustomerName;
                if (order.OrderItems != null)
                {
                    foreach (var existingItem in order.OrderItems.ToList())
                    {
                        existingItem.Book.Quantity += existingItem.Quantity;
                        OrderItemRepo.Remove(existingItem);
                    }
                    order.OrderItems = new List<OrderItems>();
                }
                int totalPrice = 0;
                foreach (var item in updateOrderDTO.createsDTO)
                {
                    var book = await BookRepo.Get(b => b.Title == item.BookTitle);
                    if (book == null)
                        return NotFound($"Book with Title {item.BookTitle} not found");
                    if (item.Quantity > book.Quantity)
                        return BadRequest($"Quantity for book {item.BookTitle} exceeds available stock");
                    order.OrderItems.Add(new OrderItems
                    {
                        BookId = book.Id,
                        Quantity = item.Quantity
                    });
                    book.Quantity -= item.Quantity; // Reduce the quantity of the book in stock
                    totalPrice += book.Price * item.Quantity;
                }
                order.TotalPrice = totalPrice;
                OrderRepo.Update(order);
                UnitOfWork.SaveAndCommit();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            Order order = await OrderRepo.Get(o => o.Id == id, Include: "OrderItems.Book");
            if (order == null)
                return NotFound($"Order with Id {id} not found");
            foreach (var item in order.OrderItems.ToList())
            {
                item.Book.Quantity += item.Quantity;
            }
            await OrderRepo.Delete(id);
            UnitOfWork.SaveAndCommit();
            return Ok("Order Deleted Successfully");
        }
    }
}

