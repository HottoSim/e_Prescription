using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class MedicationGiven
    {
        [Key]
        public int MedicationGivenId { get; set; }

        [ForeignKey("Id")]
        public int AdmissionId { get; set; }
        public virtual Admission Admission { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string MedicationName { get; set; }

        [Required(ErrorMessage = "*Required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "*Required")]
        public DateTime Time { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Instruction { get; set; }

        //public string Surgeon { get; set; }
    }
}
