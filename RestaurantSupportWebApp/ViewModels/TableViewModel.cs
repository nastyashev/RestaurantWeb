namespace RestaurantSupportWebApp.ViewModels
{
    public class TableViewModel
    {
        public int TableId { get; set; }
        public int? Hall { get; set; }
        public int Capacity { get; set; }
        public string Reservation { get; set; }
        public int? ReservationId { get; set; }
    }
}
