using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DTO.OrderDTO
{
    public class BaseOrderDTO
    {
        public string CustomerName { get; set; }
        public List<CreateOrderItemDTO> createsDTO { get; set; } = new List<CreateOrderItemDTO>();
    }
}
