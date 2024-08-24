using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class ReceivedOrderViewModel
    {
        [Required(ErrorMessage = "Please select a medication.")]
        public int SelectedMedicationId { get; set; }
        public List<SelectListItem> Medications { get; set; }

        [Required(ErrorMessage = "Please enter the quantity received.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int QuantityReceived { get; set; }

       // public int StockReceivedId { get; set; }

        public int StockOrderId { get; set; }
        public string MedicationName { get; set; }
        
        public DateTime DateReceived { get; set; }
        public string Status { get; set; }
    }
}
