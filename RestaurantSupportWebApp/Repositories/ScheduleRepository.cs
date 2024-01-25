using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantSupportWebApp.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly DataContext _context;

        public ScheduleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Schedule> GetByIdAsync(int? id)
        {
            return await _context.Schedules.FindAsync(id);
        }

        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            return await _context.Schedules.ToListAsync();
        }

        public void Add(Schedule model)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Schedule model)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Schedule model)
        {
            throw new System.NotImplementedException();
        }
    }
}
