using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class Ward
    {
        [Key]
        public int WardId { get; set; }

        [Required]
        public string WardName { get; set; }

    }
}
