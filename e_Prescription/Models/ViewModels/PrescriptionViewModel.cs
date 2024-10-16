namespace e_Prescription.Models.ViewModels
{
    public class PrescriptionViewModel
    {
        public IEnumerable<PrescriptionItem> PrescriptionItems { get; set; }
        public IEnumerable<MedicationGiven> MedicationsGiven { get; set; }

        public PrescriptionViewModel()
        {
            // Initialize the lists to prevent null reference exceptions
            PrescriptionItems = new List<PrescriptionItem>();
            MedicationsGiven = new List<MedicationGiven>();

            // Calculate the administered quantities
            foreach (var item in PrescriptionItems)
            {
                item.AdministeredQuantity = MedicationsGiven
                    .Where(mg => mg.PrescriptionItemId == item.PrescriptionItemId)
                    .Sum(mg => mg.Quantity);
            }
        }
    }
}
