using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantSupportWebApp.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly DataContext _context;

        public PositionRepository(DataContext context)
        {
            _context = context;
        }

        public Position GetPositionById(int? id)
        {
            return _context.Positions.Find(id);
        }

        public async Task<IEnumerable<Position>> GetAllAsync()
        {
            return await _context.Positions.ToListAsync();
        }

        public void Add(Position model)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Position model)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Position model)
        {
            throw new System.NotImplementedException();
        }
    }
}
