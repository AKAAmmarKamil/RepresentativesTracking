using AutoMapper;
using Modle.Model;
using DAL.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace DAL.Services
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetAll();
        Task<IEnumerable<Order>> GetAllInCompany(int Id, int PageNumber, int Count);
        Task<IEnumerable<Order>> GetAllInCompanyByStatus(int Id,int Status, int PageNumber, int Count);
        Task<IEnumerable<Order>> GetOrderByUser(int User,int PageNumber,int Count);
        Task<double?> GetOrderTotalInIQD(int OrderId);
        Task<double?> GetOrderTotalInUSD(int OrderId);
        Task<Order> GetOrderInProgress(int User);
        Task<bool> IsLastOrderCompleted(int User);
    }
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly DBContext _db;
        public OrderRepository(DBContext context) : base(context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Order>> GetAll()=> await _db.Order.Include(x => x.User).Include(x => x.Customer).ToListAsync();
        public async Task<IEnumerable<Order>> GetAllInCompany(int Id, int PageNumber, int Count) => await _db.Order.Include(x=>x.User).Include(x=>x.Customer).Where(x=>x.User.CompanyID==Id).Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();
        public async Task<IEnumerable<Order>> GetAllInCompanyByStatus(int Id,int Status, int PageNumber, int Count) => await _db.Order.Include(x => x.User).Include(x => x.Customer).Where(x => x.User.CompanyID == Id&&x.Status==Status).Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();

        public async Task<Order> FindById(int Id) => await _db.Order.Include(x=>x.User).Include(x => x.Customer).FirstOrDefaultAsync(x=>x.ID==Id);

        public async Task<IEnumerable<Order>> GetOrderByUser(int User, int PageNumber, int Count) => await _db.Order.Include(x => x.User).Include(x => x.Customer).Where(x => x.UserID == User && x.Status == 0 && x.ReceiptImageUrl == null).Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();

        public async Task<Order> GetOrderInProgress(int User) => await _db.Order.Include(x => x.User).Include(x => x.Customer).Where(x => x.UserID == User && x.Status == 1).FirstOrDefaultAsync();

        public async Task<bool> IsLastOrderCompleted(int User)
        {
          var Result=  await _db.Order.Include(x => x.User).Include(x => x.Customer).Where(x => x.UserID == User && x.Status != 0 && x.ReceiptImageUrl == null).ToListAsync();
            if (Result.Count > 0)
                return false;
            return true;
        }
        public async Task<double?> GetOrderTotalInIQD(int OrderId)=> _db.Products.Where(x=>x.OrderID==OrderId).Select(x => x.PriceInIQD * x.Quantity).Sum();
        public async Task<double?> GetOrderTotalInUSD(int OrderId)=> _db.Products.Where(x => x.OrderID == OrderId).Select(x => x.PriceInUSD * x.Quantity).Sum();
    }
}