namespace e_Prescription.Models
{
    public class MedicationOrderViewModel
    {
        public int PharmacyMedicationId { get; set; }
        public string MedicationName { get; set; }
        public int QuantityOnHand { get; set; }
        public int ReorderLevel { get; set; }
        public int DosageFormId { get; set; }
        public string DosageFormName { get; set; }

        // Fields for ordering
        public int OrderQuantity { get; set; }
        public bool IsSelected { get; set; }
    }
}
