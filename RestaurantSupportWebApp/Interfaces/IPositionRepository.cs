using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface IPositionRepository : IRepository<Position>
    {
        Position GetPositionById(int? id);
    }
}
