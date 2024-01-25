using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSupportWebApp.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public string Description { get; set; }

        public double Weight { get; set; }

        public double Price { get; set; }

        public int? PreparationTime { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public IEnumerable<MenuItem_Product> MenuItem_Products { get; set; }

        public IEnumerable<Order_MenuItem> Order_MenuItems { get; set; }
    }
}
