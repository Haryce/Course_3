using Microsoft.EntityFrameworkCore;
using SneakerShop.Data;
using SneakerShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SneakerShop.Repositories
{
    public class SneakerRepository : ISneakerRepository
    {
        private readonly ApplicationDbContext _context;

        public SneakerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sneaker>> GetAllAsync()
        {
            return await _context.Sneakers
                .Include(s => s.Brand)
                .Include(s => s.Category)
                .Where(s => s.IsAvailable)
                .ToListAsync();
        }

        public async Task<Sneaker> GetByIdAsync(int id)
        {
            return await _context.Sneakers
                .Include(s => s.Brand)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.SneakerId == id);
        }

        public async Task<IEnumerable<Sneaker>> GetByBrandAsync(int brandId)
        {
            return await _context.Sneakers
                .Include(s => s.Brand)
                .Include(s => s.Category)
                .Where(s => s.BrandId == brandId && s.IsAvailable)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sneaker>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Sneakers
                .Include(s => s.Brand)
                .Include(s => s.Category)
                .Where(s => s.CategoryId == categoryId && s.IsAvailable)
                .ToListAsync();
        }

        public async Task AddAsync(Sneaker sneaker)
        {
            await _context.Sneakers.AddAsync(sneaker);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sneaker sneaker)
        {
            _context.Sneakers.Update(sneaker);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sneaker = await GetByIdAsync(id);
            if (sneaker != null)
            {
                sneaker.IsAvailable = false;
                await UpdateAsync(sneaker);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Sneakers.AnyAsync(s => s.SneakerId == id);
        }
    }
}