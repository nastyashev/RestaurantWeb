namespace RestaurantSupportWebApp.ViewModels
{
    public class PlacementViewModel
    {
        public string CustomerName { get; set; }
        public int TableId { get; set; }
        public string Reservation { get; set; }
        public int? ReservationId { get; set; }
        public string Comment { get; set; }
    }
}
