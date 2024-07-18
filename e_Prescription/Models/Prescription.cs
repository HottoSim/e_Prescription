using e_Prescription.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string SurgeonId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Admission")]
        public int AdmissionId { get; set; }
        public virtual Admission Admission { get; set; } 

        [Required]
        public DateTime PresciptionDate { get; set; }

        [Required]
        public bool IsUrgent { get; set; } = false;

        [Required]
        public string Status { get; set; }

    }
}
