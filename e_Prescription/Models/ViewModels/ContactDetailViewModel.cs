using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.ViewModels
{
    public class ContactDetailViewModel
    {
        public int PatientId { get; set; }
        [Required(ErrorMessage = "*Required")]
        [RegularExpression(@"^(\+27|0)[6-8][0-9]{8}$", ErrorMessage = "Invalid South African phone number.")]
        public string CellphoneNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        // Other properties from ContactDetail model
        [Required(ErrorMessage = "*Required")]
        public int ProvinceId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "*Required")]
        public int SuburbId { get; set; }
        [Required(ErrorMessage = "*Required")]
        public string? StreetAddress { get; set; }
    }
}
