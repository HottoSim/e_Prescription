namespace e_Prescription.Models.ViewModels
{
    public class PrescriptionViewModel
    {
        public IEnumerable<PrescriptionItem> PrescriptionItems { get; set; }
        public IEnumerable<MedicationGiven> MedicationsGiven { get; set; }
    }
}
