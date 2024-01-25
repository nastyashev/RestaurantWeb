using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSupportWebApp.Models
{
    public class Staff : IdentityUser
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Surname { get; set; }

        [MaxLength(50)]
        public string? Patronymic { get; set; }

        public int? Salary { get; set; }

        [ForeignKey(nameof(Position))]
        public int? PositionId { get; set; }

        [ForeignKey(nameof(Schedule))]
        public int? ScheduleId { get; set; }

        public Position? Position { get; set; }

        public Schedule? Schedule { get; set; }
    }
}
