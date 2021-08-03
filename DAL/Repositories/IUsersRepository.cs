using AutoMapper;
using Model;
using Model.Form;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modle.Model;
using DAL.Repositories;
using DAL;

namespace Repository
{
    public interface IUsersRepository : IBaseRepository<User,Guid>
    {
        Task<User> Authintication(LoginForm login);
        Task<User> GetUserByEmail(string Email);
        Task<IEnumerable<User>> GetUsersByCompany(Guid Id, int PageNumber,int Count);

    }
    public class UserRepository : BaseRepository<User,Guid>, IUsersRepository
    {
        private readonly DBContext _db;
        public UserRepository(DBContext context) : base(context)
        {
            _db = context;
        }
        public async Task<User> Authintication(LoginForm login) =>
             await _db.User.Where(x => x.Email == login.EmailAddress)
                 .FirstOrDefaultAsync();
        public async Task<User> GetUserByEmail(string Email)
        {
            var result = await _db.User.Where(x => x.Email == Email)
                 .FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }
            return result;
        }
        public async Task<IEnumerable<User>> GetUsersByCompany(Guid Id, int PageNumber, int Count)
        {
            var result = await _db.User.Where(x => x.CompanyID == Id).Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();
            if (result == null)
            {
                return null;
            }
            return result;
        }
        public async Task<IEnumerable<User>> FindAll(int PageNumber,int Count) => await _db.User.Skip((PageNumber - 1) * Count).Take(Count).ToListAsync();
        public async Task<User> FindById(Guid Id)
        {
            var result = await _db.User.Where(x => x.ID == Id).Include(x=>x.Company)
                 .FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }
            return result;
        }
    }

}
