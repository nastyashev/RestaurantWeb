using Microsoft.EntityFrameworkCore;
using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly DataContext _context;

        public RestaurantRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Restaurant entity)
        {
            _context.Restaurants.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Restaurant entity)
        {
            _context.Restaurants.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Restaurant entity)
        {
            _context.Restaurants.Update(entity);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await _context.Restaurants.ToListAsync();
        }

        public async Task<Restaurant> GetByIdAsync(int id)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
