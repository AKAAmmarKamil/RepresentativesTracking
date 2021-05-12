using Modle.Model;
using RepresentativesTracking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Services
{
    public interface IOrderService : IBaseService<Order, int>
    {
        Task<IEnumerable<Order>> GetAll();
    }

    public class OrderService : IOrderService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public OrderService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }


        public async Task<Order> Create(Order Order) => await
             _repositoryWrapper.Order.Create(Order);
        public async Task<Order> Delete(int id) => await
        _repositoryWrapper.Order.Delete(id);

        public async Task<Order> FindById(int id) => await
        _repositoryWrapper.Order.FindById(id);

        public async Task<IEnumerable<Order>> GetAll()=>await _repositoryWrapper.Order.GetAll();
        public Task<IEnumerable<Order>> All(int PageNumber, int Count) => _repositoryWrapper.Order.FindAll(PageNumber, Count);
        public async Task<Order> Modify(int id, Order Order)
        {
            var CountryModelFromRepo = await _repositoryWrapper.Order.FindById(id);
            if (CountryModelFromRepo == null)
            {
                return null;
            }
            CountryModelFromRepo.Details = Order.Details;
            CountryModelFromRepo.StartLongitude = Order.StartLongitude;
            CountryModelFromRepo.StartLatitude = Order.StartLatitude;
            CountryModelFromRepo.ReceiptImageUrl = Order.ReceiptImageUrl;
            CountryModelFromRepo.EndLongitude = Order.EndLatitude;
            CountryModelFromRepo.EndLatitude = Order.EndLatitude;
            CountryModelFromRepo.UserID = Order.UserID;
            _repositoryWrapper.Save();
            return Order;
        }

    }
}