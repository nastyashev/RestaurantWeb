using Microsoft.EntityFrameworkCore;
using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;

namespace RestaurantSupportWebApp.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly DataContext _context;

        public ReservationRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Reservation entity)
        {
            _context.Reservations.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Reservation entity)
        {
            _context.Reservations.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservations.ToListAsync();
        }

        public void Update(Reservation entity)
        {
            _context.Reservations.Update(entity);
            _context.SaveChanges();
        }
    }
}
