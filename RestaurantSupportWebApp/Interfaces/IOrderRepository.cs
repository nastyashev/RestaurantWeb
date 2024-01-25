using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order_MenuItem>> GetAllWithDetaisAsync();
        Task<Order_MenuItem> GetOrderDetailsById(int id);
        void UpdateOrderDetails(Order_MenuItem order_MenuItemChanges);
        Task<IEnumerable<Order>> GetOrdersByTableId(int tableId);
    }
}
