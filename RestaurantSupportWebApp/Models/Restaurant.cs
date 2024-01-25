using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;

namespace RestaurantSupportWebApp.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public string Schedule { get; set; }

        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }

        public Address Address { get; set; }

        public IEnumerable<Table> Tables { get; set; }
    }
}
