using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class StockReceived
    {
        [Key]
        public int StockReceivedId { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("StockOrder")]
        public int StockOrderId { get; set; }
        public virtual StockOrder StockOrder { get; set; }

        [Required(ErrorMessage = "Please enter the quantity received.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int QuantityReceived { get; set; }

        [Required(ErrorMessage = "*Required")]
        public DateTime Date { get; set; }

        public virtual ICollection<StockOrder> StockOrders { get; set; } = new List<StockOrder>();
    }
}
