using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.Account
{
    public class UpdateUsersViewModel
    {


        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }

        // Role-specific properties
        //public string HPCSANumber { get; set; }
        public string Specialization { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
        //public string PharmacyLicenseNumber { get; set; }
        //public string NursingLicenseNumber { get; set; }
    }
}
