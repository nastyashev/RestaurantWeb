using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantSupportWebApp.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Client entity)
        {
            _context.Clients.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Client entity)
        {
            _context.Clients.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Client entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }
    }
}
