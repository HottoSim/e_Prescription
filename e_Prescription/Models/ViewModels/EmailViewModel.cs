namespace e_Prescription.Models.ViewModels
{
    public class EmailViewModel
    {
        public int AdmissionId { get; set; }
        public string PatientName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
