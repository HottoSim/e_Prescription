using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class PatientCondition
    {
        [Key]
        public int PatientConditionId { get; set; }

        [ForeignKey("PatientId")]
        public int PatientId { get; set; }

        [ForeignKey("ChronicConditionId")]
        public int? ChronicConditionId { get; set; } = null;
        public virtual ChronicCondition ChronicCondition { get; set; }
    }
}
