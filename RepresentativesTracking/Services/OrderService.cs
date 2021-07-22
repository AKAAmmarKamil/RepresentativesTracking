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
        Task<IEnumerable<Order>> GetOrderByUser(int User, int PageNumber, int Count);
        Task<bool> IsLastOrderCompleted(int User);
        Task<Order> StartModify(int id, Order Order);
        Task<Order> EndModify(int id, Order Order);
        Task<Order> GetOrderInProgress(int User);
        Task<double?> GetOrderTotalInIQD(int OrderId);
        Task<double?> GetOrderTotalInUSD(int OrderId);
    }

    public class OrderService : IOrderService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public OrderService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task<IEnumerable<Order>> GetOrderByUser(int User, int PageNumber, int Count) =>await 
            _repositoryWrapper.Order.GetOrderByUser(User, PageNumber, Count);
        public async Task<double?> GetOrderTotalInIQD(int OrderId) => await
           _repositoryWrapper.Order.GetOrderTotalInIQD(OrderId);
        public async Task<double?> GetOrderTotalInUSD(int OrderId) => await
           _repositoryWrapper.Order.GetOrderTotalInUSD(OrderId);
        public async Task<bool> IsLastOrderCompleted(int User) => await
           _repositoryWrapper.Order.IsLastOrderCompleted(User);
        public async Task<Order> Create(Order Order) => await
             _repositoryWrapper.Order.Create(Order);
        public async Task<Order> Delete(int id) => await
        _repositoryWrapper.Order.Delete(id);

        public async Task<Order> FindById(int id) => await
        _repositoryWrapper.Order.FindById(id);
        public async Task<Order> GetOrderInProgress(int id) => await
       _repositoryWrapper.Order.GetOrderInProgress(id);
        public async Task<IEnumerable<Order>> GetAll()=>await _repositoryWrapper.Order.GetAll();
        public Task<IEnumerable<Order>> All(int PageNumber, int Count) => _repositoryWrapper.Order.FindAll(PageNumber, Count);
        public async Task<Order> StartModify(int id, Order Order)
        {
            var OrderModelFromRepo = await _repositoryWrapper.Order.FindById(id);
            if (OrderModelFromRepo == null)
            {
                return null;
            }
            OrderModelFromRepo.StartLongitude = Order.StartLongitude;
            OrderModelFromRepo.StartLatitude = Order.StartLatitude;
            OrderModelFromRepo.ISInProgress = true;
            _repositoryWrapper.Save();
            return Order;
        }
        public async Task<Order> EndModify(int id, Order Order)
        {
            var OrderModelFromRepo = await _repositoryWrapper.Order.FindById(id);
            if (OrderModelFromRepo == null)
            {
                return null;
            }
            OrderModelFromRepo.ReceiptImageUrl = Order.ReceiptImageUrl;
            OrderModelFromRepo.ISInProgress = false ;
            OrderModelFromRepo.DeliveryOrderDate = DateTime.Now;
            _repositoryWrapper.Save();
            return Order;
        }
        public async Task<Order> Modify(int id, Order Order)
        {
            var OrderModelFromRepo = await _repositoryWrapper.Order.FindById(id);
            if (OrderModelFromRepo == null)
            {
                return null;
            }
            OrderModelFromRepo.Details = Order.Details;
            OrderModelFromRepo.StartLongitude = Order.StartLongitude;
            OrderModelFromRepo.StartLatitude = Order.StartLatitude;
            OrderModelFromRepo.ReceiptImageUrl = Order.ReceiptImageUrl;
            OrderModelFromRepo.EndLongitude = Order.EndLatitude;
            OrderModelFromRepo.EndLatitude = Order.EndLatitude;
            OrderModelFromRepo.UserID = Order.UserID;
            _repositoryWrapper.Save();
            return Order;
        }
    }
}