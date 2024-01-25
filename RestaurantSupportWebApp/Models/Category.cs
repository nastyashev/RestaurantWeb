using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public string Characteristics { get; set; }

        public IEnumerable<MenuItem> MenuItems { get; set; }
    }
}
