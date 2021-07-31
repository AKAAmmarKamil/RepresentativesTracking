using Modle.Model;
using DAL.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace DAL.Services
{
    public interface ILocationRepository : IBaseRepository<RepresentativeLocation>
    {
        Task<IEnumerable<RepresentativeLocation>> GetAllByOrder(int User,int Order);
        Task<RepresentativeLocation> GetLastOfUser(int User);
        Task<IEnumerable<RepresentativeLocation>> GetAllBetweenTwoDates(int User,DateTime Start, DateTime End);
    }
    public class LocationRepository : BaseRepository<RepresentativeLocation>, ILocationRepository
    {
        private readonly DBContext _db;
        public LocationRepository(DBContext context) : base(context)
        {
            _db = context;
        }
        public async Task<IEnumerable<RepresentativeLocation>> FindById() => await _db.Location.Include(x => x.Order).Include(x=>x.User).ToListAsync();

        public async Task<IEnumerable<RepresentativeLocation>> GetAllByOrder(int User,int Order)=> await _db.Location.Where(x=>x.UserID==User&&x.OrderID==Order).ToListAsync();
        public async Task<IEnumerable<RepresentativeLocation>> GetAllBetweenTwoDates(int User,DateTime Start, DateTime End) => await _db.Location.Where(x => x.UserID == User && x.LocationDate>= Start&&x.LocationDate<=End).ToListAsync();
        public async Task<RepresentativeLocation> GetLastOfUser(int User) => await _db.Location.Where(x => x.UserID == User).OrderByDescending(x=>x.LocationDate).Take(1).FirstOrDefaultAsync();

    }
}
