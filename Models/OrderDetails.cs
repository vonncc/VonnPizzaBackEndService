

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VonnPizzaBackEndService.Models
{
    public class OrderDetails
    {
        [Key]
        public required int OrderDetailID { get; set; }
        
        public required int Quantity { get; set; }

        [ForeignKey("OrderID")]
        public Orders Order { get; set; }
        [ForeignKey("PizzaID")]
        public Pizzas Pizza { get; set; }

    }
}
