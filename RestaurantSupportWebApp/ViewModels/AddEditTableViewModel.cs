using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.ViewModels
{
    public class AddEditTableViewModel
    {
        public int TableId { get; set; }

        [Range(1, 3, ErrorMessage = "Зал повинен бути від 1 до 3")]
        public int? Hall { get; set; }

        [Required(ErrorMessage = "Введіть кількість місць")]
        [Range(1, 20, ErrorMessage = "Кількість місць повинна бути від 1 до 20")]
        public int NumberOfSeats { get; set; }

        [Required(ErrorMessage = "Виберіть ресторан")]
        public int RestaurantId { get; set; }
    }
}
