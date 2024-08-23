namespace e_Prescription.Models
{
    public class StockOrderViewModel
    {
        public int StockOrderId { get; set; }
        public string MedicationName { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
