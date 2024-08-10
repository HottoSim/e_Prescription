namespace e_Prescription.Models
{
    public class AdmissionViewModel
    {
        public int AdmissionId { get; set; }
        public List<PatientsVitals> PatientsVitals { get; set; } = new List<PatientsVitals>();

        public string NurseId { get; set; }
    }
}
