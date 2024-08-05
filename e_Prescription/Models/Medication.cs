using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string MedicationName { get; set; }
    }
}
