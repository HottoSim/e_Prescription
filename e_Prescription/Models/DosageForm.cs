using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class DosageForm
    {
        [Key]
        public int DosageFormId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
