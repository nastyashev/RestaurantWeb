using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSupportWebApp.Models
{
    public class ProductInCart
    {
        [Key]
        public int Id { get; set; }
        public int Amount { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
