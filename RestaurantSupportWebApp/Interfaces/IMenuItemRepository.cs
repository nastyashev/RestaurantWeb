using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        Task<MenuItem> GetByIdAsync(int id);
        Task<MenuItem> GetByTitleAsync(string title);
        Task<IEnumerable<MenuItem>> GetAllWithCategories();
        Task<IEnumerable<MenuItem>> GetAllWithDetails();
        void AddProductToMenuItem(MenuItem_Product menuItem_Product);
        Task<IEnumerable<MenuItem_Product>> GetProductsByMenuItemIdAsync(int id);
        void DeleteProductFromMenuItem(MenuItem_Product menuItem_Product);
        void AddMenuItemToOrder(Order_MenuItem order_MenuItem);
    }
}
