using Microsoft.AspNetCore.Mvc.Rendering;

namespace e_Prescription.Models
{
    public class ViewPrescriptionViewModel
    {
        public PrescriptionItem PrescriptionItem { get; set; }
        public SelectList StatusOptions { get; set; }
    }
}
