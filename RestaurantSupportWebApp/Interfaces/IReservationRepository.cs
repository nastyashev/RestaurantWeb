using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<Reservation> GetByIdAsync(int id);
    }
}
