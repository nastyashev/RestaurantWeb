using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<Schedule> GetByIdAsync(int? id);
    }
}
