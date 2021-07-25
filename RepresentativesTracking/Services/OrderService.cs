using Microsoft.AspNetCore.Http;
using Modle.Model;
using RepresentativesTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Services
{
    public interface IOrderService : IBaseService<Order, int>
    {
        Task<IEnumerable<Order>> GetAll();
        Task<IEnumerable<Order>> GetAllByCompany(int Id, int PageNumber, int Count);
        Task<IEnumerable<Order>> GetOrderByUser(int User, int PageNumber, int Count);
        Task<bool> IsLastOrderCompleted(int User);
        Task<Order> StartModify(int id, Order Order);
        Task<Order> EndModify(int id, Order Order);
        Task<Order> GetOrderInProgress(int User);
        Task<double?> GetOrderTotalInIQD(int OrderId);
        Task<double?> GetOrderTotalInUSD(int OrderId);
        string GetURL(string Image);
        Task<List<Order>> SortOrders(double Longitude, double Latitude, List<Order> Destination);
    }
        public class OrderService : IOrderService
        {
            private readonly IRepositoryWrapper _repositoryWrapper;
            private readonly IHttpContextAccessor _httpContextAccessor;
            public OrderService(IRepositoryWrapper repositoryWrapper, IHttpContextAccessor httpContextAccessor)
            {
                _repositoryWrapper = repositoryWrapper;
                _httpContextAccessor = httpContextAccessor;
            }
            public async Task<IEnumerable<Order>> GetOrderByUser(int User, int PageNumber, int Count) => await
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
            public async Task<IEnumerable<Order>> GetAll() => await _repositoryWrapper.Order.GetAll();
            public Task<IEnumerable<Order>> All(int PageNumber, int Count) => _repositoryWrapper.Order.FindAll(PageNumber, Count);
            public Task<IEnumerable<Order>> GetAllByCompany(int Id,int PageNumber, int Count) => _repositoryWrapper.Order.GetAllByCompany(Id,PageNumber, Count);

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
                OrderModelFromRepo.ISInProgress = false;
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
            public string GetURL(string Image)
            {
                string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                var IsHttps = _httpContextAccessor.HttpContext.Request.IsHttps;
                var Http = "https://";
                if (IsHttps == false)
                {
                    Http = "http://";
                }
                return Http + host + "/Images/" + Image + ".jpeg";
            }
            public async Task<List<double>> Getdisplacement(double Longitude, double Latitude, List<Order> Destination)
            {
                var DisplacementInLongitude = 0.0;
                var DisplacementInLatitude = 0.0;
                var DisplacementList = new List<double>();
                for (int i = 0; i < Destination.Count; i++)
                {
                    DisplacementInLongitude = Math.Pow(Longitude - Destination[i].EndLongitude, 2);
                    DisplacementInLatitude = Math.Pow(Latitude - Destination[i].EndLatitude, 2);
                    DisplacementList.Add(Math.Sqrt(DisplacementInLongitude + DisplacementInLatitude));
                }
                return DisplacementList;
            }
            public async Task<List<Order>> SortOrders(double Longitude, double Latitude, List<Order> Destination)
            {
                var DisplacementList = await Getdisplacement(Longitude, Latitude, Destination);
                var Dictionary = new Dictionary<Order, double>();
                for (int i = 0; i < Destination.Count; i++)
                {
                    Dictionary.Add(Destination[i], DisplacementList[i]);
                }
                var Ordered = Dictionary.OrderBy(Key => Key.Value).Select(x => x.Key).ToList();
                return Ordered;
            }
        }
    }