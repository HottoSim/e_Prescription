using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.ViewModels
{
    public class PatientMedicationReportViewModel
    {
        public int PatientId { get; set; }
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Display(Name = "Admission Date")]
        public DateTime AdmissionDate { get; set; }

        [Display(Name = "Medication Name")]
        public string MedicationName { get; set; }

        [Display(Name = "Quantity Given")]
        public int QuantityGiven { get; set; }

        [Display(Name = "Time")]
        public DateTime Time { get;set; }

        [Display(Name = "Date Range Start")]
        [DataType(DataType.Date)]
        public DateTime DateRangeStart { get; set; }

        [Display(Name = "Date Range End")]
        [DataType(DataType.Date)]
        public DateTime DateRangeEnd { get; set; }
    }
}
