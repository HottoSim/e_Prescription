using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using e_Prescription.Data;
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

        [ForeignKey("ApplicationUser")]
        public string SurgeonId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required(ErrorMessage = "*Required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string TimeSlot { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("Theatre")]
        public int TheatreId { get; set; }
        public virtual Theatre Theatre { get; set; }
    }
}
