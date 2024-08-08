using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; }

        [Required]
        public string MedicationName { get; set; }
        //[Required]
        //public string Dosage { get; set; }
        //[Required]
        //public string Schedule { get; set; }
        //[Required]
        //public int ReOrderLevel { get; set; }
    }
}
