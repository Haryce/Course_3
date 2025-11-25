using Microsoft.EntityFrameworkCore;
using SneakerShop.Data;
using SneakerShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SneakerShop.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }
    }
}