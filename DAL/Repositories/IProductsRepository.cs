using Modle.Model;
using DAL.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
namespace DAL.Services
{
    public interface IProductsRepository : IBaseRepository<Products>
    {
        Task<List<Products>> GetAll(int PageNumber, int Count);
        Task<Products> FindById(int Id);
        Task<List<Products>> GetAllByOrder(int OrderId, int PageNumber, int Count);
    }
    public class ProductsRepository : BaseRepository<Products>, IProductsRepository
    {
        private readonly DBContext _db;
        public ProductsRepository(DBContext context) : base(context)
        {
            _db = context;
        }
        public async Task<List<Products>> GetAll(int PageNumber,int Count) => await _db.Products.Include(x => x.Order).ThenInclude(x => x.User).Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();
        public async Task<List<Products>> GetAllByOrder(int OrderId, int PageNumber, int Count) => await _db.Products.Where(x=>x.OrderID==OrderId).Include(x=>x.Order).ThenInclude(x=>x.User).Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();
        public async Task<Products> FindById(int Id) => await _db.Products.Include(x => x.Order).ThenInclude(x => x.User).ThenInclude(x=>x.Company).FirstOrDefaultAsync(x => x.Id == Id);
    }
}