using RestaurantSupportWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.ViewModels
{
    public class AddOrderProductViewModel
    {
        [Required(ErrorMessage = "Це поле обов'язкове")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове")]
        [Display(Name = "Кількість")]
        public int Amount { get; set; }
    }
}
