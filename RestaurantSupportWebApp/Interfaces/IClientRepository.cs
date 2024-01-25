using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> GetByIdAsync(int id);
    }
}
