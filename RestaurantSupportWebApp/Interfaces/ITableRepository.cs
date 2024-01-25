using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface ITableRepository : IRepository<Table>
    {
        Task<Table> GetByIdAsync(int id);
        Task<IEnumerable<Table>> GetTablesByHallAsync(int hall);
    }
}
