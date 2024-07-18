using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class MedicationGiven
    {
        [Key]
        public int MedicationGivenId { get; set; }

        [ForeignKey("PrescriptionItem")]
        public int PrescriptionItemId { get; set; }
        public virtual PrescriptionItem PrescriptionItem { get; set; }


        [Required(ErrorMessage = "*Required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "*Required")]
        public DateTime Time { get; set; }

       
    }
}
