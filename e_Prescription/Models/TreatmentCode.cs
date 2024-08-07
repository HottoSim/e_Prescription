using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class TreatmentCode
    {
        [Key]
        public int TreatmentCodeId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string ICD_10_CODE { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string TreatmentCodeDescription { get; set; }
    }
}
