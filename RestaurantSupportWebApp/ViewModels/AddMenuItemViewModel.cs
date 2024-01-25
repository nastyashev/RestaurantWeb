using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.ViewModels
{
    public class AddMenuItemViewModel
    {
        [MaxLength(50)]
        public string Title { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
        public int? PreparationTime { get; set; }
        public int CategoryId { get; set; }

        public List<ProductInMenuItemViewModel> Products { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове")]
        [Display(Name = "Вага продукту")]
        public double ProductWeight { get; set; }
    }
}
