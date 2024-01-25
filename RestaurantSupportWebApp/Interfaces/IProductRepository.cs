using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetByIdAsync(int id);
        Task<Product> GetByProductNameAsync(string productName);
    }
}
