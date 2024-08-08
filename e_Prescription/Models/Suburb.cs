using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class Suburb
    {
        [Key]
        public int SuburbId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string SuburbName { get; set; }

        // City foreign key
        [Required(ErrorMessage = "*Required")]
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        [Required(ErrorMessage ="*Required")]
        public string PostalCode { get; set; }
    }
}
