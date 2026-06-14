using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model
{
    public class Book : BaseModel
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public string AuthorName { get; set; }
        public int Quantity { get; set; }

        public List<OrderItems>? OrderItems { get; set; }

    }
}
