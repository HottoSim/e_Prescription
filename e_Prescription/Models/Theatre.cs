using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class Theatre
    {
        [Key]
        public int TheatreId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string TheatreName { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Description { get; set; }
    }
}
