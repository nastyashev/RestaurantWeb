using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSupportWebApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string Comment { get; set; }

        [ForeignKey(nameof(Table))]
        public int TableId { get; set; }

        [ForeignKey(nameof(Waiter))]
        public string WaiterId { get; set; }

        public bool IsPaid { get; set; }

        public Table Table { get; set; }

        public Staff Waiter { get; set; }

        public IEnumerable<Order_MenuItem> Order_MenuItems { get; set; }
    }
}
