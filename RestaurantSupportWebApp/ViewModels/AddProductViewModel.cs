using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.ViewModels
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage = "Це поле обов'язкове")]
        [MaxLength(100)]
        [Display(Name = "Назва")]
        public string Title { get; set; }

        public bool IsAllergen { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове")]
        [Display(Name = "Ціна")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ціна не може бути меншою за 0.01")]
        public double Price { get; set; }
    }
}
