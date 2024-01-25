using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSupportWebApp.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int NumberOfPeople { get; set; }

        public string Comment { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        [ForeignKey(nameof(Table))]
        public int TableId { get; set; }

        public Client Client { get; set; }

        public Table Table { get; set; }
    }
}
