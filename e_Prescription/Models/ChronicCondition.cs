using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class ChronicCondition
    {
        [Key]
        public int ChronicCondotionId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string ConditionName { get; set; }
    }
}
