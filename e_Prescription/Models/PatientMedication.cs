using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class PatientMedication
    {
        [Key]
        public int PatientMedicationId { get; set; }

        [ForeignKey("PatientId")]
        public int PatientId { get; set; }

        [ForeignKey("MedicationId")]
        public int? MedicationId { get; set; } = null;
    }
}
