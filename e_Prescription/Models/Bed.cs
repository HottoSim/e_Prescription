using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class Bed
    {
        [Key]
        public int BedId { get; set; }

        [Required]
        public string BedName { get; set; }

        [ForeignKey("WardId")]
        public int WardId { get; set; }
        public virtual Ward Ward { get; set; }

        [Required]
        public bool IsAvailable { get; set; } = true;

        //[Required]
        //public string Name { get; set; }
    }
}
