using AutoMapper;
using Modle.Model;
using DAL.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace DAL.Services
{
    public interface ICompanyRepository : IBaseRepository<Company,Guid>
    {
        Task<IEnumerable<Company>> GetAll();
    }
    public class CompanyRepository : BaseRepository<Company,Guid>, ICompanyRepository
    {
        private readonly DBContext _db;
        public CompanyRepository(DBContext context) : base(context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Company>> GetAll()=> await _db.Company.ToListAsync();
    }
}
