using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DTO.BookDTO
{
    public class GetBookDTO : BaseBookDTO
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } 
    }
}
