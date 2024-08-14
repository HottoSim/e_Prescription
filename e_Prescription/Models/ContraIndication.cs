using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class ContraIndication
    {
        [Key]
        public int IndicationId { get; set; }

        [ForeignKey("ChronicCondition")]
        public int ChronicConditionId { get; set; }
        public virtual ChronicCondition ChronicCondition { get; set; }

        [ForeignKey("Ingredient")]
        public int ActiveIngredientId { get; set; }
        public virtual ActiveIngredient ActiveIngredient { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Description { get; set; }
    }
}
