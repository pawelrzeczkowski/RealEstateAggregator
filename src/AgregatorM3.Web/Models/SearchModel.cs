namespace AgregatorM3.Web.Models
{
    public class SearchModel
    {
        public int PriceFrom { get; set; } = 400000;
        public int PriceTo { get; set; } = 950000;
        public int SurfaceFrom { get; set; } = 60;
        public int SurfaceTo { get; set; } = 100;
        public int RoomsFrom { get; set; } = 3;
        public int RoomsTo { get; set; } = 5;
        public int PricePerMeterTo { get; set; } = 13000;
    }
}
