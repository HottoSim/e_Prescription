using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class PatientVital
    {
        public PatientVital()
        {

        }

        [Key]
        public int PatientVitalId { get; set; }

        [ForeignKey("Id")]
        public int AdmissionId { get; set; }
        public virtual Admission Admission { get; set; }

        [Required(ErrorMessage = "*Required")]
        [Range(1.0, Double.MaxValue, ErrorMessage = "Reading must be a positive number.")]
        public double? Reading { get; set; }

        [Required(ErrorMessage = "*Required")]
        public DateTime Time { get; set; }

        [ForeignKey("VitalId")]
        public int VitalId { get; set; }

        public virtual Vital Vital { get; set; }

        public string? Note { get; set; } = "Normal";
    }
}
