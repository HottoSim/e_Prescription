namespace e_Prescription.Models.Account
{
    public class UserCountViewModel
    {
        public int NurseCount { get; set; }

        public int SurgeonCount { get; set; }

        public int PharmacistCount { get; set; }

        public int AdminCount { get; set; }

        //Not Active User Count
        public int NoNurseCount { get; set; }

        public int NoSurgeonCount { get; set; }

        public int NoPharmacistCount { get; set; }

        public int NoAdminCount { get; set; }
    }
}
