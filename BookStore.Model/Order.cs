using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model
{
    public class Order : BaseModel
    {
        public int TotalPrice { get; set; }
        public string CustomerName { get; set; }

        public List<OrderItems>? OrderItems { get; set; }
    }
}
