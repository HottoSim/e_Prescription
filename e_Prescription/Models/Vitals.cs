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
        public double? LowLimit { get; set; } = 0.0;

        [Required(ErrorMessage = "*Required")]
        public double? HighLimit { get; set; } = 0.0;

        [Required(ErrorMessage = "*Required")]
        public string Units { get; set; }

        public string? LowLimitDiastolic { get; set; }  // Optional for blood pressure
        public string? HighLimitDiastolic { get; set; } // Optional for blood pressure
    }
}
