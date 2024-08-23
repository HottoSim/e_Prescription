namespace e_Prescription.Models.ViewModels
{
    public class EmailViewModel
    {
        public string FromEmail { get; set; } // Automatically filled with the logged-in user's email
        public string ToEmail { get; set; }   // Selected by the user
        public string Subject { get; set; }   // User input
        public string Message { get; set; }   // Populated with data from the Admission table
        public int AdmissionId { get; set; }  // The Admission record to pull data from

        public virtual Admission Admission { get; set; }
    }
}
