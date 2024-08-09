using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class Vitals
    {
        [Key]
        public int VitalId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string VitalName { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string LowLimit { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string HighLimit { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Units { get; set; }

    }
}
