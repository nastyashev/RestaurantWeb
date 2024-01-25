using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public IEnumerable<Staff> Staff { get; set; }
    }
}
