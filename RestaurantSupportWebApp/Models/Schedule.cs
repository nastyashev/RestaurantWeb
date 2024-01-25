using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace RestaurantSupportWebApp.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ScheduleJson { get; set; }
    }
}
