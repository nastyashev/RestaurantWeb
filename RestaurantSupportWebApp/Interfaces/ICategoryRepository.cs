using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetByIdAsync(int id);
        Task<Category> GetByCategoryNameAsync(string categoryName);
    }
}
