using System.ComponentModel.DataAnnotations;

namespace RestaurantSupportWebApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public int AmountInStock { get; set; }

        public bool IsAllergen { get; set; }

        public double Price { get; set; }

        public IEnumerable<MenuItem_Product> MenuItem_Products { get; set; }
    }
}
