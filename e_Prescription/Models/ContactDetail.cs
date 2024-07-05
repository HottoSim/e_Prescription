using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class ContactDetail
    {
        [Key]
        public int ContactId { get; set; }

        [ForeignKey("PatientId")]
        public int PatientId { get; set; } // Foreign key to link with Patient
        public virtual Patient Patient { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("Province")]
        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }

        // City foreign key
        [Required(ErrorMessage = "*Required")]
        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        // Suburb foreign key
        [Required(ErrorMessage = "*Required")]
        [ForeignKey("Suburb")]
        public int SuburbId { get; set; }
        public virtual Suburb Suburb { get; set; }

        [Required]
        [RegularExpression(@"^(\+27|0)[6-8][0-9]{8}$", ErrorMessage = "Invalid South African phone number.")]
        public int CellphoneNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        public string? StreetAddress { get; set; }

    }
}
