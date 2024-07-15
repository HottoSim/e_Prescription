using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class TreatmentCode
    {
        [Key]
        public int TreatmentCodeId { get; set; }

        [Required]
        public string TreatmentCodeName { get; set; }

        [Required]
        public string TreatmentCodeDescription { get; set; }
    }
}
