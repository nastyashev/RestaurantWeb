using RestaurantSupportWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.ViewModels
{
    public class OrderProductsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
    }
}
