using e_Prescription.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class Admission
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PatientId")]
        public int PatientId { get; set; } // Foreign key to link with Patient
        public virtual Patient Patient { get; set; }

        [Required(ErrorMessage = "*Required")]
        public DateTime AdmissionDate { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("Ward")]
        public int WardId { get; set; }
        public virtual Ward Ward { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("Bed")]
        public int BedId { get; set; }
        public virtual Bed Bed { get; set; }

        [Required(ErrorMessage = "*Required")]
        public double? Weight { get; set; }

        [Required(ErrorMessage = "*Required")]
        public double? Height { get; set; }

        public virtual List<PatientsVitals> PatientsVitals { get; set; } = new List<PatientsVitals>();
        public virtual List<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        [Required]
        public bool IsDischarged { get; set; } = false;

        [Required(ErrorMessage = "*Required")]
        public double? BMI { get; set; }

        [ForeignKey("ApplicationUser")]
        public string NurseId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
