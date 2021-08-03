using Modle.Model;
using DAL.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace DAL.Services
{
    public interface ICustomerRepository : IBaseRepository<Customer,Guid>
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<IEnumerable<Customer>> GetCustomersByCompany(Guid CompanyId,int PageNumber,int Count);
        Task<Customer> GetById(Guid Id, Guid CompanyId);
    }
    public class CustomerRepository : BaseRepository<Customer,Guid>, ICustomerRepository
    {
        private readonly DBContext _db;
        public CustomerRepository(DBContext context) : base(context)
        {
            _db = context;
        }
        public async Task<Customer> GetById(Guid Id,Guid CompanyId) => await _db.Customer.Where(x=>x.Id==Id&&x.CompanyID==CompanyId).Include(x => x.Company).FirstOrDefaultAsync();
        public async Task<IEnumerable<Customer>> GetAll()=> await _db.Customer.Include(x=>x.Company).ToListAsync();
        public async Task<IEnumerable<Customer>> GetCustomersByCompany(Guid CompanyId, int PageNumber,int Count) => await _db.Customer.Where(x=>x.CompanyID==CompanyId).Include(x => x.Company).Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();

    }
}