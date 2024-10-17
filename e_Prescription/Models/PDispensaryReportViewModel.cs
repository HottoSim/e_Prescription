namespace e_Prescription.Models
{
    public class PDispensaryReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<PrescriptionItem> Prescriptions { get; set; } = new List<PrescriptionItem>(); // Initialize to avoid null
        public int TotalDispensed { get; set; }
        public int TotalRejected { get; set; }
        public IEnumerable<dynamic> SummaryPerMedication { get; set; } // Holds the medication summary
        public string PharmacistName { get; set; } // Add this property
    }
}
