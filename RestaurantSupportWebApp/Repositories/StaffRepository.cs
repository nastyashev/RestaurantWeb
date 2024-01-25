using RestaurantSupportWebApp.Interfaces;
using RestaurantSupportWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace RestaurantSupportWebApp.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly DataContext _context;

        public StaffRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Staff>> GetAllAsync()
        {
            return await _context.Staffs.ToListAsync();
        }

        public async Task<Staff> GetByIdAsync(string id)
        {
            return await _context.Staffs.FindAsync(id);
        }

        public void Add(Staff staff)
        {
            _context.Staffs.AddAsync(staff);
            _context.SaveChanges();
        }

        public void Delete(Staff staff)
        {
            _context.Staffs.Remove(staff);
            _context.SaveChanges();
        }

        public void Update(Staff staff)
        {
            _context.Staffs.Update(staff);
            _context.SaveChanges();
        }
    }
}
