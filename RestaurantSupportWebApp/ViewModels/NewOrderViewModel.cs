using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.ViewModels
{
    public class NewOrderViewModel
    {
        [Required(ErrorMessage = "Це поле обов'язкове")]
        public int TableId { get; set; }

        public string Comment { get; set; }

        public List<MealInOrderViewModel> Meals { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове")]
        public string MealTitle { get; set; }

        public string MealComment { get; set; }
    }
}
