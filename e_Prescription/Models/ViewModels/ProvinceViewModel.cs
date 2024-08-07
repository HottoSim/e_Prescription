namespace e_Prescription.Models.ViewModels
{
    public class ProvinceViewModel
    {
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public List<CityViewModel> Cities { get; set; } = new List<CityViewModel>();
    }
}
