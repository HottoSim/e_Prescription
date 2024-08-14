using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class MedicationInteraction
    {
        [Key]
        public int InterationId { get; set; }

        [ForeignKey("Ingredient")]
        public int ActiveIngredientId { get; set; }
        public virtual ActiveIngredient ActiveIngredient { get; set; }
    }
}
