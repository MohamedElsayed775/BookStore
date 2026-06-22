using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DTO.BookDTO
{
    public class BaseBookDTO
    {
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
