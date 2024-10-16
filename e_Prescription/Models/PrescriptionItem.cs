using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class PrescriptionItem
    {
        [Key]
        public int PrescriptionItemId { get; set; }

        [ForeignKey("Prescription")]
        public int PrescriptionId { get; set; }
        public virtual Prescription Prescription { get; set; }

        [ForeignKey("PharmacyMedication")]
        public int MedicationId { get; set; }
        public virtual PharmacyMedication PharmacyMedication { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Instruction { get; set; }

        public string? RejectionNote { get; set; }

        public virtual List<MedicationGiven> MedicationGiven { get; set; } = new List<MedicationGiven>();

        public int AdministeredQuantity { get; set; }
    }
}
