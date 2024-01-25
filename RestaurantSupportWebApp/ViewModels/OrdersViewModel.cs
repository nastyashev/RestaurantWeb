namespace RestaurantSupportWebApp.ViewModels
{
    public class OrdersViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemTitle { get; set; }
        public string Comment { get; set; }
        public bool IsReady { get; set; }
    }
}
