

using System.ComponentModel.DataAnnotations;

namespace VonnPizzaBackEndService.Models
{
    public interface Orders
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime OrderDateTime { get; set; }


        
    }
}
