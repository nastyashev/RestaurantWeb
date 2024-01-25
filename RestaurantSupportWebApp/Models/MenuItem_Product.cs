using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSupportWebApp.Models
{
    public class MenuItem_Product
    {
        [Key]
        public int Id { get; set; }

        public int ProductWeight { get; set; }

        [ForeignKey(nameof(MenuItem))]
        public int MenuItemId { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public MenuItem MenuItem { get; set; }

        public Product Product { get; set; }
    }
}
