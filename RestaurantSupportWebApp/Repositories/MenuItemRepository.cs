using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantSupportWebApp.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly DataContext _context;

        public MenuItemRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(MenuItem entity)
        {
            _context.MenuItems.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(MenuItem entity)
        {
            _context.MenuItems.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<MenuItem> GetByIdAsync(int id)
        {
            return await _context.MenuItems.FindAsync(id);
        }

        public async Task<MenuItem> GetByTitleAsync(string title)
        {
            return await _context.MenuItems.FirstOrDefaultAsync(m => m.Title == title);
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _context.MenuItems.ToListAsync();
        }

        public void Update(MenuItem entity)
        {
            _context.MenuItems.Update(entity);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<MenuItem>> GetAllWithCategories()
        {
            return await _context.MenuItems
                .Include(m => m.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetAllWithDetails()
        {
            return await _context.MenuItems
                .Include(m => m.Category)
                .Include(m => m.MenuItem_Products)
                .ToListAsync();
        }

        public void AddProductToMenuItem(MenuItem_Product menuItem_Product)
        {
            _context.MenuItem_Products.Add(menuItem_Product);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<MenuItem_Product>> GetProductsByMenuItemIdAsync(int id)
        {
            return await _context.MenuItem_Products
                .Where(m => m.MenuItemId == id)
                .Include(m => m.Product)
                .ToListAsync();
        }

        public void DeleteProductFromMenuItem(MenuItem_Product menuItem_Product)
        {
            _context.MenuItem_Products.Remove(menuItem_Product);
            _context.SaveChanges();
        }

        public void AddMenuItemToOrder(Order_MenuItem order_MenuItem)
        {
            _context.Order_MenuItems.Add(order_MenuItem);
            _context.SaveChanges();
        }
    }
}
