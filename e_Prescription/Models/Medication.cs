using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string MedicationName { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("Dosage")]
        public int DosageFormId { get; set; }
        public virtual DosageForm DosageForm { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Schedule { get; set; }

        public virtual MedicationIngredient MedicationIngredient { get; set; }
    }
}
