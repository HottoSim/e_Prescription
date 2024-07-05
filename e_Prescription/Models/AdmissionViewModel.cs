namespace e_Prescription.Models
{
    public class AdmissionViewModel
    {
        public int AdmissionId { get; set; }
        public List<PatientVital> PatientVitals { get; set; } = new List<PatientVital>();

        public string NurseId { get; set; }
    }
}
