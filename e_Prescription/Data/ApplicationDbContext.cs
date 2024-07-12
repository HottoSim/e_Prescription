using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using e_Prescription.Models;
using e_Prescription.Models.Account;

namespace e_Prescription.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        public DbSet<Admission> Admissions { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Bed> Beds { get; set; }

        public DbSet<PatientVital> PatientVitals { get; set; }

        public DbSet<Vital> Vitals { get; set; }

        public DbSet<PatientAllergies> PatientAllergies { get; set; }

        public DbSet<ActiveIngredient> ActiveIngredients { get; set; }

        public DbSet<ChronicCondition> ChronicConditions { get; set; }

        public DbSet<PatientCondition> PatientConditions { get; set; }

        public DbSet<Medication> Medications { get; set; }

        public DbSet<PatientMedication> PatientMedications { get; set; }

        public DbSet<ContactDetail> ContactDetails { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Suburb> Suburbs { get; set; }

        public DbSet<Ward> Wards { get; set; }

        public DbSet<MedicationGiven> MedicationsGiven { get; set; }

        public DbSet<Surgeon> Surgeons { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<PatientBooking> PatientBookings { get; set; }

        public DbSet<Discharge> Discharges { get; set; }

        public DbSet<Nurse> Nurses { get; set; }

        public DbSet<Pharmacist> Pharmacists { get; set; }

    }
}
