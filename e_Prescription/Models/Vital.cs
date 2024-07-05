using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class Vital
    {
        [Key]
        public int VitalId { get; set; }

        [Required]
        public string VitalName { get; set; }

        [Required]
        public double LowLimit { get; set; }

        [Required]
        public double HighLimit { get; set; }

        [Required]
        public string Units { get; set; }
    }
}
