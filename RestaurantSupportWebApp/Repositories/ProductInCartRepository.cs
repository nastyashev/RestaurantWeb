using Microsoft.EntityFrameworkCore;
using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Repositories
{
    public class ProductInCartRepository : IProductInCartRepository
    {
        private readonly DataContext _context;

        public ProductInCartRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductInCart>> GetAllAsync()
        {
            return await _context.ProductsInCart.ToListAsync();
        }

        public async Task<ProductInCart> GetByProductNameAsync(string productName)
        {
            return await _context.ProductsInCart.FirstOrDefaultAsync(p => p.Product.Name == productName);
        }

        public async Task<ProductInCart> GetByIdAsync(int id)
        {
            return await _context.ProductsInCart.FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Add(ProductInCart entity)
        {
            _context.ProductsInCart.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(ProductInCart entity)
        {
            _context.ProductsInCart.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(ProductInCart entity)
        {
            _context.ProductsInCart.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteAll()
        {
            _context.ProductsInCart.RemoveRange(_context.ProductsInCart);
            _context.SaveChanges();
        }
    }
}
