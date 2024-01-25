using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.ViewModels
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public string Category { get; set; }
    }
}
