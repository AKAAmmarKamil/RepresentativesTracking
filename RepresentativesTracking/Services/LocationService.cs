using Modle.Model;
using RepresentativesTracking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Services
{
    public interface ILocationService : IBaseService<RepresentativeLocation, int>
    {
        Task<IEnumerable<RepresentativeLocation>> GetAllByOrder(int User, int Order);
        Task<IEnumerable<RepresentativeLocation>> GetAllBetweenTwoDates(int User, DateTime Start, DateTime End);
    }

    public class LocationService : ILocationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public LocationService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<IEnumerable<RepresentativeLocation>> All(int PageNumber, int Count)=>_repositoryWrapper.Location.FindAll(PageNumber, Count);
        public async Task<RepresentativeLocation> Create(RepresentativeLocation Location) => await
             _repositoryWrapper.Location.Create(Location);
        public async Task<RepresentativeLocation> Delete(int id) => await
        _repositoryWrapper.Location.Delete(id);

        public async Task<RepresentativeLocation> FindById(int id) => await
        _repositoryWrapper.Location.FindById(id);
        public async Task<IEnumerable<RepresentativeLocation>> GetAllByOrder(int User, int Order) => await
        _repositoryWrapper.Location.GetAllByOrder(User,Order); 
        public async Task<IEnumerable<RepresentativeLocation>> GetAllBetweenTwoDates(int User, DateTime Start, DateTime End) => await
         _repositoryWrapper.Location.GetAllBetweenTwoDates(User,Start,End);
        public Task<RepresentativeLocation> Modify(int id, RepresentativeLocation t)
        {
            throw new NotImplementedException();
        }
    }
}