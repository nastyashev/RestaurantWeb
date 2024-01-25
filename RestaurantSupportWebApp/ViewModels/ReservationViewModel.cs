using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.ViewModels
{
    public class ReservationViewModel
    {
        [Required(ErrorMessage = "Введіть ім'я клієнта")]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "Введіть номер телефону клієнта")]
        [Phone(ErrorMessage = "Номер телефону повинен бути у форматі +380XXXXXXXXX")]
        [RegularExpression(@"^ *\+?\d{2}[ -]*\d *\d{2}[ -]*\d{3}[ -]*\d{2}[ -]*\d{2} *$", ErrorMessage = "Номер телефону повинен бути у форматі +380XXXXXXXXX")]
        public string ClientPhone { get; set; }

        [Required(ErrorMessage = "Введіть дату бронювання")]
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Введіть час закінчення бронювання")]
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Введіть кількість людей")]
        public int NumberOfPeople { get; set; }

        [Required(ErrorMessage = "Виберіть зал")]
        public int Hall { get; set; }

        [Required(ErrorMessage = "Виберіть столик")]
        public int TableId { get; set; }

        public string? Comment { get; set; }

        public override string ToString()
        {
            return $"ClientName: {ClientName}, ClientPhone: {ClientPhone}, StartTime: {StartTime}, EndTime: {EndTime}, NumberOfPeople: {NumberOfPeople}, Hall: {Hall}, TableId: {TableId}, Comment: {Comment}";
        }
    }
}
