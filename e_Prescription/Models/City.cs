using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("Province")]
        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }

        public virtual ICollection<Suburb> Suburbs { get; set; }
    }
}
