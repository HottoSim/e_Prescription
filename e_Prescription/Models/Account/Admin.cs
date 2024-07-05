using e_Prescription.Data;
using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.Account
{
    public class Admin
    {
        [Key]
        public int UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
