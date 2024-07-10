using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class PatientAllergies
    {
        [Key]
        public int PatientAllergyId { get; set; }

        [ForeignKey("PatientId")]
        public int PatientId { get; set; }

        [ForeignKey("ActiveIngredientId")]
        public int? ActiveIngredientId { get; set; }
    }
}
