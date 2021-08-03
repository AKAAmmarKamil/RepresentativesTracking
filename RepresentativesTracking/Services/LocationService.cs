using Modle.Model;
using RepresentativesTracking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Services
{
    public interface ILocationService : IBaseService<RepresentativeLocation, Guid>
    {
        Task<IEnumerable<RepresentativeLocation>> GetAllByOrder(Guid User, Guid Order);
        Task<IEnumerable<RepresentativeLocation>> GetAllBetweenTwoDates(Guid User, DateTime Start, DateTime End);
        Task<RepresentativeLocation> GetLastOfUser(Guid User);
    }

    public class LocationService : ILocationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public LocationService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public Task<IEnumerable<RepresentativeLocation>> All(int PageNumber,int Count)=>_repositoryWrapper.Location.FindAll(PageNumber, Count);
        public Task<RepresentativeLocation> GetLastOfUser(Guid User) => _repositoryWrapper.Location.GetLastOfUser(User);
        public async Task<RepresentativeLocation> Create(RepresentativeLocation Location) => await
             _repositoryWrapper.Location.Create(Location);
        public async Task<RepresentativeLocation> Delete(Guid id) => await
        _repositoryWrapper.Location.Delete(id);
        public async Task<RepresentativeLocation> FindById(Guid id) => await
        _repositoryWrapper.Location.FindById(id);
        public async Task<IEnumerable<RepresentativeLocation>> GetAllByOrder(Guid User, Guid Order) => await
        _repositoryWrapper.Location.GetAllByOrder(User,Order); 
        public async Task<IEnumerable<RepresentativeLocation>> GetAllBetweenTwoDates(Guid User, DateTime Start, DateTime End) => await
         _repositoryWrapper.Location.GetAllBetweenTwoDates(User,Start,End);
        public Task<RepresentativeLocation> Modify(Guid id, RepresentativeLocation t)
        {
            throw new NotImplementedException();
        }
    }
}