using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Model;

namespace BookStore.DTO.OrderDTO
{
    public class GetOrderDTO : BaseOrderDTO
    {
        public int TotalPrice { get; set; }  
    }
}
