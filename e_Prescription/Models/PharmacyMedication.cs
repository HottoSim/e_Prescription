﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Prescription.Models
{
    public class PharmacyMedication
    {
        [Key]
        public int PharmacyMedicationId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string MedicationName { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Schedule { get; set; }

        [Required(ErrorMessage = "*Required")]
        public int QuantityOnHand { get; set; }

        [Required(ErrorMessage = "*Required")]
        public int ReorderLevel { get; set; }

        [Required(ErrorMessage = "*Required")]
        [ForeignKey("DosageForm")]
        public int DosageFormId { get; set; }
        public virtual DosageForm DosageForm { get; set; }


        public virtual ICollection<PharmacyMedicationIngredient> PharmacyMedicationIngredients { get; set; }
    }
}
