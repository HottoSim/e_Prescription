namespace e_Prescription.Models
{
    public class MedicationOrderViewModel
    {
        public List<PharmacyMedicationViewModel> Medications { get; set; }
        public List<StockOrderViewModel> StockOrders { get; set; }
    }
}
