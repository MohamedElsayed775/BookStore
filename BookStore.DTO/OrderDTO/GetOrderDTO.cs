using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Model;

namespace BookStore.DTO.OrderDTO
{
    public class GetOrderDTO
    {
        public string CustomerName { get; set; }
        public int TotalPrice { get; set; }
        public List<CreateOrderItemDTO> createsDTO { get; set; } = new List<CreateOrderItemDTO>();
    }
}
