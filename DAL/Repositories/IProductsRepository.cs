using Modle.Model;
using DAL.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace DAL.Services
{
    public interface IProductsRepository : IBaseRepository<Products,Guid>
    {
        Task<List<Products>> GetAll(int PageNumber, int Count);
        Task<Products> FindById(Guid Id);
        Task<List<Products>> GetAllByOrder(Guid OrderId, int PageNumber, int Count);
    }
    public class ProductsRepository : BaseRepository<Products,Guid>, IProductsRepository
    {
        private readonly DBContext _db;
        public ProductsRepository(DBContext context) : base(context)
        {
            _db = context;
        }
        public async Task<List<Products>> GetAll(int PageNumber,int Count) => await _db.Products.Include(x => x.Order).ThenInclude(x => x.User).Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();
        public async Task<List<Products>> GetAllByOrder(Guid OrderId, int PageNumber, int Count) => await _db.Products.Where(x=>x.OrderID==OrderId).Include(x=>x.Order).ThenInclude(x=>x.User).Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();
        public async Task<Products> FindById(Guid Id) => await _db.Products.Include(x => x.Order).ThenInclude(x => x.User).ThenInclude(x=>x.Company).FirstOrDefaultAsync(x => x.Id == Id);
    }
}