using SneakerShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SneakerShop.Services
{
    public interface ISneakerService
    {
        Task<List<Sneaker>> GetAllSneakersAsync();
        Task<Sneaker> GetSneakerByIdAsync(int id);
        Task<List<Sneaker>> GetSneakersByBrandAsync(string brandName);
        Task<List<Sneaker>> GetSneakersByCategoryAsync(string categoryName);
        Task AddSneakerAsync(Sneaker sneaker);
        Task UpdateSneakerAsync(Sneaker sneaker);
        Task DeleteSneakerAsync(int id);
        Task<List<Brand>> GetAllBrandsAsync();
        Task<List<Category>> GetAllCategoriesAsync();
    }
}

// Services/SneakerService.cs
using SneakerShop.Models;
using SneakerShop.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SneakerShop.Services
{
    public class SneakerService : ISneakerService
    {
        private readonly ISneakerRepository _sneakerRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;

        public SneakerService(ISneakerRepository sneakerRepository, 
                            IBrandRepository brandRepository,
                            ICategoryRepository categoryRepository)
        {
            _sneakerRepository = sneakerRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Sneaker>> GetAllSneakersAsync()
        {
            var sneakers = await _sneakerRepository.GetAllAsync();
            return sneakers.ToList();
        }

        public async Task<Sneaker> GetSneakerByIdAsync(int id)
        {
            return await _sneakerRepository.GetByIdAsync(id);
        }

        public async Task<List<Sneaker>> GetSneakersByBrandAsync(string brandName)
        {
            var brands = await _brandRepository.GetAllAsync();
            var brand = brands.FirstOrDefault(b => b.Name.ToLower() == brandName.ToLower());
            
            if (brand == null) return new List<Sneaker>();
            
            var sneakers = await _sneakerRepository.GetByBrandAsync(brand.BrandId);
            return sneakers.ToList();
        }

        public async Task<List<Sneaker>> GetSneakersByCategoryAsync(string categoryName)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var category = categories.FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());
            
            if (category == null) return new List<Sneaker>();
            
            var sneakers = await _sneakerRepository.GetByCategoryAsync(category.CategoryId);
            return sneakers.ToList();
        }

        public async Task AddSneakerAsync(Sneaker sneaker)
        {
            await _sneakerRepository.AddAsync(sneaker);
        }

        public async Task UpdateSneakerAsync(Sneaker sneaker)
        {
            await _sneakerRepository.UpdateAsync(sneaker);
        }

        public async Task DeleteSneakerAsync(int id)
        {
            await _sneakerRepository.DeleteAsync(id);
        }

        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            return brands.ToList();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.ToList();
        }
    }
}