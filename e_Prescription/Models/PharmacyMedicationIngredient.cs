using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class PharmacyMedicationIngredient
    {
        [Key]
        public int PharmacyMedicationIngredientId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public int MedicationId { get; set; }
        public virtual PharmacyMedication PharmacyMedication { get; set; }

        [Required(ErrorMessage = "*Required")]
        public int ActiveIngredientId { get; set; }
        public virtual ActiveIngredient ActiveIngredient { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Strength { get; set; }
    }
}
