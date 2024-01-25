using Microsoft.EntityFrameworkCore;
using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Category entity)
        {
            _context.Categories.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Category entity)
        {
            _context.Categories.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Category entity)
        {
            _context.Categories.Update(entity);
            _context.SaveChanges();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByCategoryNameAsync(string categoryName)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Title == categoryName);
        }
    }
}
