using Microsoft.EntityFrameworkCore;
using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly DataContext _context;

        public TableRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Table entity)
        {
            _context.Tables.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Table entity)
        {
            _context.Tables.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<Table> GetByIdAsync(int id)
        {
            return await _context.Tables.FindAsync(id);
        }

        public async Task<IEnumerable<Table>> GetAllAsync()
        {
            return await _context.Tables.ToListAsync();
        }

        public void Update(Table entity)
        {
            _context.Tables.Update(entity);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Table>> GetTablesByHallAsync(int hall)
        {
            return await _context.Tables.Where(t => t.Hall == hall).ToListAsync();
        }
    }
}
