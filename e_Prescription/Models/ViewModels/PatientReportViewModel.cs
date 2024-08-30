using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models.ViewModels
{
    public class PatientReportViewModel
    {
        public int PatientId { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string IdNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public string Status { get; set; }

        public List<PatientAllergies> Allergies { get; set; } = new List<PatientAllergies>();

        public List<PatientCondition> Conditions { get; set; } = new List<PatientCondition>();

        public List<PatientMedication> Medications { get; set; } = new List<PatientMedication>();

        public PatientBooking Booking { get; set; }

        public Admission Admission { get; set; }

        public Discharge Discharge { get; set; }

        public List<PatientsVitals> Vitals { get; set; } = new List<PatientsVitals>();

        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        public List<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();
    }
}
