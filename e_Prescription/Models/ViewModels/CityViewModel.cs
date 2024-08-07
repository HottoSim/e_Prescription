namespace e_Prescription.Models.ViewModels
{
    public class CityViewModel
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public List<SuburbViewModel> Suburbs { get; set; } = new List<SuburbViewModel>();
    }
}
