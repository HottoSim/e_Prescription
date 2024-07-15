using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class MedicationIngredient
    {
        [Key]
        public int MedicationIngredientId { get; set; }

        [ForeignKey("Medication")]
        public int MedicationId { get; set; }
        public virtual Medication Medication { get; set; }

        [ForeignKey("ActiveIngredient")]
        public int ActiveIngredientId { get; set; }
        public virtual ActiveIngredient ActiveIngredient { get; set; }

        [Required]
        public string IngredientStrength { get; set; }
    }
}
