using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class BookingTreatmentViewModel
    {
        public int BookingId { get; set; }
        public int PatientId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public int treatmentCodeId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string timeSlot { get; set; }

        [Required(ErrorMessage = "*Required")]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "*Required")]
        public int theatreId { get; set; }
        public string surgeonId { get; set; }
    }
}
