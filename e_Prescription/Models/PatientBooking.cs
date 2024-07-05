using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using e_Prescription.Models.Account;

namespace e_Prescription.Models
{
    public class PatientBooking
    {
        [Key]
        public int BookingId { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; } // Foreign key to link with Patient
        public virtual Patient Patient { get; set; }

        [ForeignKey("SurgeonId")]
        public int HealthcareProfessionalId { get; set; }
        public virtual HealthcareProfessional HealthcareProfessional { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string TimeSlot { get; set; }

        [Required]
        public string TreatmentCode { get; set; }

        [Required]
        public string Theatre { get; set; }
    }
}
