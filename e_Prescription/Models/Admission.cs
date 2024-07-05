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

        public DateTime AdmissionDate { get; set; }= DateTime.Now;

        [Required(ErrorMessage = "*required")]
        [ForeignKey("Ward")]
        public int WardId { get; set; }
        public virtual Ward Ward { get; set; }

        [Required(ErrorMessage = "*required")]
        [ForeignKey("Bed")]
        public int BedId { get; set; }
        public virtual Bed Bed { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Invalid weight entered")]
        public double Weight { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Invalid Height entered.")]
        public double Height { get; set; }

        public virtual List<PatientVital> PatientVitals { get; set; } = new List<PatientVital>();

        [Required]
        public bool IsDischarged { get; set; } = false;

        [ForeignKey("ApplicationUser")]
        public string NurseId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
