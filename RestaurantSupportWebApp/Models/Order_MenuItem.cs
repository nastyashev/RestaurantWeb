using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSupportWebApp.Models
{
    public class Order_MenuItem
    {
        [Key]
        public int Id { get; set; }

        public bool IsReady { get; set; }

        public string Comment { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        [ForeignKey(nameof(MenuItem))]
        public int MenuItemId { get; set; }

        public Order Order { get; set; }

        public MenuItem MenuItem { get; set; }
    }
}
