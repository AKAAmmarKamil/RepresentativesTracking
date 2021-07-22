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

        public async Task<IEnumerable<Order>> GetAll()=> await _db.Order.ToListAsync();
        public async Task<IEnumerable<Order>> FindById() => await _db.Order.Include(x=>x.User).ToListAsync();

        public async Task<IEnumerable<Order>> GetOrderByUser(int User, int PageNumber, int Count) => await _db.Order.Where(x => x.UserID == User && x.ISInProgress == false && x.ReceiptImageUrl == null).Skip((PageNumber - 1) * Count).Take(Count).OrderBy(x=>x.AddOrderDate).ToListAsync();

        public async Task<Order> GetOrderInProgress(int User) => await _db.Order.Where(x => x.UserID == User && x.ISInProgress == true).FirstOrDefaultAsync();

        public async Task<bool> IsLastOrderCompleted(int User)
        {
          var Result=  await _db.Order.Where(x => x.UserID == User && x.ISInProgress == true && x.ReceiptImageUrl == null).ToListAsync();
            if (Result.Count > 0)
                return false;
            return true;
        }
        public async Task<double?> GetOrderTotalInIQD(int OrderId)=> _db.Products.Where(x=>x.OrderID==OrderId).Select(x => x.PriceInIQD * x.Count).Sum();
        public async Task<double?> GetOrderTotalInUSD(int OrderId)=> _db.Products.Where(x => x.OrderID == OrderId).Select(x => x.PriceInUSD * x.Count).Sum();
    }
}