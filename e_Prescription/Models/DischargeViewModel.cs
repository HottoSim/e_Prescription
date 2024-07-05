using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class DischargeViewModel
    {
        public int AdmissionId { get; set; }

        [Required(ErrorMessage = "*required")]
        public string DischargeNote { get; set; }

        [Required]
        public DateTime DischargeTime { get; set; } = DateTime.Now;

        public string NurseId { get; set; }
    }
}
