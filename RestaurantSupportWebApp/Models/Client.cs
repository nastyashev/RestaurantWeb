using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public long PhoneNumber { get; set; }

        public int Discount { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
