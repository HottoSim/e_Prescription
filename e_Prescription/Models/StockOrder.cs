using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class StockOrder
    {
        [Key]
        public int StockOrderId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "*Required")]
        public int OrderQuantity { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Status { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("PharmacyMedicationId")]
        public int PharmacyMedicationId { get; set; } // Corrected
        public virtual PharmacyMedication PharmacyMedication { get; set; }
    }
}
 