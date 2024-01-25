using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.ViewModels
{
    public class StaffViewModel
    {
        public string Id { get; set; }
        public required string FullName { get; set; }
        public string ScheduleName { get; set; }
    }
}
