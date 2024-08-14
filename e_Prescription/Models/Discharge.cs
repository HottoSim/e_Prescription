using e_Prescription.Data;
using e_Prescription.Models.Account;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class Discharge
    {
        [Key]
        public int DischargeId { get; set; }

        [Required(ErrorMessage ="*required")]
        public string DischargeNote { get; set; }

        [Required(ErrorMessage = "*required")]
        public DateTime DischargeTime { get; set; }

        [ForeignKey("Id")]
        public int AdmissionId { get; set; }
        public virtual Admission Admission { get; set; }

        //[ForeignKey("NurseId")]
        //public string Nurse { get; set; }
        //public virtual HealthcareProfessional HealthcareProfessional { get; set; }

        [ForeignKey("ApplicationUser")]
        public string NurseId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
