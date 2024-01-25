using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSupportWebApp.Models
{
    public class Table
    {
        [Key]
        public int Id { get; set; }

        public int NumberOfSeats { get; set; }

        public int? Hall { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
