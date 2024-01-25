using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantSupportWebApp.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            this._context = context;
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Update(Order orderChanges)
        {
            _context.Orders.Update(orderChanges);
            _context.SaveChanges();
        }

        public void Delete(Order order)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Order_MenuItem>> GetAllWithDetaisAsync()
        {
            return await _context.Order_MenuItems.ToListAsync();
        }

        public async Task<Order_MenuItem> GetOrderDetailsById(int id)
        {
            return await _context.Order_MenuItems.FindAsync(id);
        }

        public void UpdateOrderDetails(Order_MenuItem order_MenuItemChanges)
        {
            _context.Order_MenuItems.Update(order_MenuItemChanges);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Order>> GetOrdersByTableId(int tableId)
        {
            return await _context.Orders.Where(o => o.TableId == tableId).ToListAsync();
        }
    }
}
