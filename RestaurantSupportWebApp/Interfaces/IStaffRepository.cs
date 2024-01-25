using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<Staff> GetByIdAsync(string id);
    }
}
