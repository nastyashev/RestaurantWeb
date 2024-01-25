using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        Task<Restaurant> GetByIdAsync(int id);
    }
}
