using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.ViewModels
{
    public class PatientMedicationGroupViewModel
    {
        public string PatientName { get; set; }
        public DateTime AdmissionDate { get; set; }
        public List<PatientMedicationReportViewModel> Medications { get; set; }

        [Display(Name = "Date Range Start")]
        [DataType(DataType.Date)]
        public DateTime DateRangeStart { get; set; }

        [Display(Name = "Date Range End")]
        [DataType(DataType.Date)]
        public DateTime DateRangeEnd { get; set; }

        public int PatientId { get; set; }
    }
}
