using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BookStore.Model
{
    public class OrderItems : BaseModel
    {
        public int Quantity { get; set; }   //quantity of the book in the order


        [ForeignKey("Book")]
        public int BookId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public Book? Book { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }
    }
}
