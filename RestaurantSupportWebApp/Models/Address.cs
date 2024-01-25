using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(10)]
        public string Office { get; set; }

        [MaxLength(10)]
        public string Building { get; set; }

        [MaxLength(20)]
        public string Street { get; set; }

        [MaxLength(20)]
        public string City { get; set; }

        [MaxLength(20)]
        public string Country { get; set; }

        public IEnumerable<Restaurant> Restaurants { get; set; }
    }
}
