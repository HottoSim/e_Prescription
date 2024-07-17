using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class Vital
    {
        [Key]
        public int VitalId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string VitalName { get; set; }

        [Required(ErrorMessage = "*Required")]
        public double LowLimit { get; set; }

        [Required(ErrorMessage = "*Required")]
        public double HighLimit { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Units { get; set; }
    }
}
