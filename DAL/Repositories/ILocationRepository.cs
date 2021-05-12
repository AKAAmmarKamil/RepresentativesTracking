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
        Task<IEnumerable<RepresentativeLocation>> GetAllBetweenTwoDates(int User,DateTime Start, DateTime End);
    }
    public class LocationRepository : BaseRepository<RepresentativeLocation>, ILocationRepository
    {
        private readonly DBContext _db;
        public LocationRepository(DBContext context) : base(context)
        {
            _db = context;
        }

        public async Task<IEnumerable<RepresentativeLocation>> GetAllByOrder(int User,int Order)=> await _db.Location.Where(x=>x.UserID==User&&x.OrderID==Order).ToListAsync();
        public async Task<IEnumerable<RepresentativeLocation>> GetAllBetweenTwoDates(int User,DateTime Start, DateTime End) => await _db.Location.Where(x => x.UserID == User && x.LocationDate>= Start&&x.LocationDate<=End).ToListAsync();

    }
}
