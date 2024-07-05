using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class Province
    {
        [Key]
        public int ProvinceId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string ProvinceName { get; set; }
    }
}
