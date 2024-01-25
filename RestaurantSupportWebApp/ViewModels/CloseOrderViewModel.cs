using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.ViewModels
{
    public class CloseOrderViewModel
    {
        [Required(ErrorMessage = "Це поле обов'язкове")]
        public int TableId { get; set; }

        public List<MealInOrderViewModel> Meals { get; set; }
    }
}
