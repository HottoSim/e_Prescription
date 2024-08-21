using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class PharmacyMedicationIngredient
    {
        [Key]
        public int PharmacyMedicationIngredientId { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("PharmacyMedicationId")]
        public int PhamacyMedicationId { get; set; }
        public virtual PharmacyMedication PharmacyMedication { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("ActiveIngredientId")]
        public int ActiveIngredientId { get; set; }
        public virtual ActiveIngredient ActiveIngredient { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Strength { get; set; }

    }
}
