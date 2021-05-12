using AutoMapper;
using Modle.Model;
using DAL.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace DAL.Services
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetAll();
        Task<IEnumerable<Order>> GetOrderByUser(int User,int PageNumber,int Count);

    }
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly DBContext _db;
        public OrderRepository(DBContext context) : base(context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Order>> GetAll()=> await _db.Order.ToListAsync();
        public async Task<IEnumerable<Order>> GetOrderByUser(int User, int PageNumber, int Count) => await _db.Order.Where(x=>x.UserID==User && x.ReceiptImageUrl==null).Skip((PageNumber - 1) * Count).Take(Count).OrderBy(x=>x.AddOrderDate).ToListAsync();
    }
}
