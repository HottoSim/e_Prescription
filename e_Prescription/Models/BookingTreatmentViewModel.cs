namespace e_Prescription.Models
{
    public class BookingTreatmentViewModel
    {
        public int treatmentCodeId { get; set; }
        public string timeSlot { get; set; }
        public DateTime BookingDate { get; set; }
        public int theatreId { get; set; }
        public string surgeonId { get; set; }
    }
}
