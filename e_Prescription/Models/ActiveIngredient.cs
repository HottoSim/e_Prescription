using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class ActiveIngredient
    {
        [Key]
        public int ActiveIngredientId { get; set; }

        [Required(ErrorMessage ="*Required")]
        public string IngredientName { get; set; }
    }
}
