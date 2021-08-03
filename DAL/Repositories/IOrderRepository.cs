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
    public interface IOrderRepository : IBaseRepository<Order,Guid>
    {
        Task<IEnumerable<Order>> GetAll();
        Task<IEnumerable<Order>> GetAllInCompany(Guid Id, int PageNumber, int Count);
        Task<IEnumerable<Order>> GetAllInCompanyByStatus(Guid Id,int Status, int PageNumber, int Count);
        Task<IEnumerable<Order>> GetOrderByUser(Guid User,int PageNumber,int Count);
        Task<double?> GetOrderTotalInIQD(Guid OrderId);
        Task<double?> GetOrderTotalInUSD(Guid OrderId);
        Task<Order> GetOrderInProgress(Guid User);
        Task<bool> IsLastOrderCompleted(Guid User);
    }
    public class OrderRepository : BaseRepository<Order,Guid>, IOrderRepository
    {
        private readonly DBContext _db;
        public OrderRepository(DBContext context) : base(context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Order>> GetAll()=> await _db.Order.Include(x => x.User).Include(x => x.Customer).ToListAsync();
        public async Task<IEnumerable<Order>> GetAllInCompany(Guid Id, int PageNumber, int Count) => await _db.Order.Include(x=>x.User).Include(x=>x.Customer).Where(x=>x.User.CompanyID==Id).Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();
        public async Task<IEnumerable<Order>> GetAllInCompanyByStatus(Guid Id,int Status, int PageNumber, int Count) => await _db.Order.Include(x => x.User).Include(x => x.Customer).Where(x => x.User.CompanyID == Id&&x.Status==Status).Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();

        public async Task<Order> FindById(Guid Id) => await _db.Order.Include(x=>x.User).Include(x => x.Customer).FirstOrDefaultAsync(x=>x.ID==Id);

        public async Task<IEnumerable<Order>> GetOrderByUser(Guid User, int PageNumber, int Count) => await _db.Order.Include(x => x.User).Include(x => x.Customer).Where(x => x.UserID == User && x.Status == 0 && x.ReceiptImageUrl == null).Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();

        public async Task<Order> GetOrderInProgress(Guid User) => await _db.Order.Include(x => x.User).Include(x => x.Customer).Where(x => x.UserID == User && x.Status == 1).FirstOrDefaultAsync();

        public async Task<bool> IsLastOrderCompleted(Guid User)
        {
          var Result=  await _db.Order.Include(x => x.User).Include(x => x.Customer).Where(x => x.UserID == User && x.Status != 0 && x.ReceiptImageUrl == null).ToListAsync();
            if (Result.Count > 0)
                return false;
            return true;
        }
        public async Task<double?> GetOrderTotalInIQD(Guid OrderId)=> _db.Products.Where(x=>x.OrderID==OrderId).Select(x => x.PriceInIQD * x.Quantity).Sum();
        public async Task<double?> GetOrderTotalInUSD(Guid OrderId)=> _db.Products.Where(x => x.OrderID == OrderId).Select(x => x.PriceInUSD * x.Quantity).Sum();
    }
}