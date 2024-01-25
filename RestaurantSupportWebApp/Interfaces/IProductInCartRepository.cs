using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface IProductInCartRepository : IRepository<ProductInCart>
    {
        Task<ProductInCart> GetByProductNameAsync(string productName);
        Task<ProductInCart> GetByIdAsync(int id);
        void DeleteAll();
    }
}
