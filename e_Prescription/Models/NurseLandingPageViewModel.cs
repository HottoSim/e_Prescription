namespace e_Prescription.Models
{
    public class NurseLandingPageViewModel
    {
        public string NurseName { get; set; }
        public int AdmittedPatientsCount { get; set; }
        public int BookedPatientsCount { get; set; }

        public int AdmittedCount { get; set; }
        public int DischargedCount { get; set; }
        public int BookedPatients { get; set; }
    }
}
