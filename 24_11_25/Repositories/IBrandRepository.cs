using SneakerShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SneakerShop.Repositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand> GetByIdAsync(int id);
    }
}