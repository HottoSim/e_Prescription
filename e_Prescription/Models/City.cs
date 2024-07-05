using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        public string CityName { get; set; }

        [Required]
        public int PostalCode { get; set; }

        [ForeignKey("Province")]
        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }
    }
}
