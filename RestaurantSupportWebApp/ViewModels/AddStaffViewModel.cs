using System.ComponentModel.DataAnnotations;
using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.ViewModels
{
    public class AddStaffViewModel
    {
        [Required(ErrorMessage = "Це поле обов'язкове")]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове")]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string Surname { get; set; }

        [MaxLength(50)]
        [Display(Name = "Patronymic")]
        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове")]
        [MaxLength(50)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове")]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Salary")]
        [Range(0, int.MaxValue, ErrorMessage = "Salary must be positive")]
        public int? Salary { get; set; }

        public int? ScheduleId { get; set; }

        public int? PositionId { get; set; }
    }
}
