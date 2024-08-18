using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class BookingTreatment
    {
        [Key]
        public int BookingTreatmentId { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("PatientBooking")]
        public int BookingId { get; set; }
        public virtual PatientBooking PatientBooking { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("TreatmentCode")]
        public int TreatmentCodeId { get; set; }
        public virtual TreatmentCode TreatmentCode { get; set; }

    }
}
