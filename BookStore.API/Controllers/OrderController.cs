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

        public OrderController(IRepository<Order> _orderRepo, IRepository<Book> _bookRepo , UnitOfWorks _unitOfWork )
        {
            OrderRepo = _orderRepo;
            BookRepo = _bookRepo;
            UnitOfWork = _unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            // include Book navigation so item.Book is populated
            List<Order> orders = (await OrderRepo.GetAllAsync(Include: "OrderItems.Book")).ToList();
            List<GetOrderDTO> getOrderDTOs = new List<GetOrderDTO>();
            foreach (var order in orders)
            {
                GetOrderDTO getOrderDTO = new GetOrderDTO()
                {
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

                order.OrderItems.Add(new OrderItems
                {
                    BookId = book.Id,
                    OrderId = order.Id,
                    Quantity = item.Quantity
                });

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

    }
}

