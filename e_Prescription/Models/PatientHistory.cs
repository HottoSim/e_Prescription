using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class PatientHistory
    {
        public int PatientId { get; set; }

        public Patient Patient { get; set; } // This property holds the patient information

        public string PatientName { get; set; }

        public List<ActiveIngredient> Allergies { get; set; } // Assuming ActiveIngredient is the model for allergies

        public List<int> SelectedAllergies { get; set; } = new List<int>();// Stores the IDs of selected allergies

        public List<ChronicCondition> ChronicConditions { get; set; }

        public List<int> SelectedConditions { get; set; } = new List<int>();

        public List<Medication> Medications { get; set; }

        public List<int> SelectedMedications { get; set; } = new List<int>();
    }

}
