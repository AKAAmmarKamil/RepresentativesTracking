using Modle.Model;
using Newtonsoft.Json.Linq;
using RepresentativesTracking;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Services
{
    public interface IProductsService : IBaseService<Products, Guid>
    {
        Task<List<Products>> GetAll(int PageNumber,int Count);
        Task<Products> FindById(Guid Id);
        Task<List<Products>> GetAllByOrder(Guid OrderId, int PageNumber,int Count);
        double CurrencyConverting(double Amount, bool ToUSD);
        Task<double> Convert(Guid CompantId, double Amount, bool ToUSD);
    }
    public class ProductsService : IProductsService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public ProductsService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public Task<IEnumerable<Products>> All(int PageNumber,int Count)=>_repositoryWrapper.Products.FindAll(PageNumber, Count);
        public async Task<Products> Create(Products Products) => await
             _repositoryWrapper.Products.Create(Products);
        public async Task<Products> Delete(Guid id) => await
        _repositoryWrapper.Products.Delete(id);
        public async Task<Products> FindById(Guid id) => await
        _repositoryWrapper.Products.FindById(id);
        public async Task<List<Products>> GetAll(int PageNumber,int Count) =>await _repositoryWrapper.Products.GetAll(PageNumber,Count);
        public async Task<List<Products>> GetAllByOrder(Guid OrderId, int PageNumber,int Count) => await _repositoryWrapper.Products.GetAllByOrder(OrderId,PageNumber,Count);
        public async Task<Products> Modify(Guid id, Products Products)
        {
            var ProductsModelFromRepo = await _repositoryWrapper.Products.FindById(id);
            if (ProductsModelFromRepo == null)
            {
                return null;
            }
            ProductsModelFromRepo.Name = Products.Name;
            ProductsModelFromRepo.Quantity = Products.Quantity;
            ProductsModelFromRepo.PriceInIQD = Products.PriceInIQD;
            ProductsModelFromRepo.PriceInUSD = Products.PriceInUSD;
            ProductsModelFromRepo.OrderID = Products.OrderID;
            _repositoryWrapper.Save();
            return Products;
        }
        public double CurrencyConverting(double Amount, bool ToUSD)
        {
            var client = new RestClient("https://v6.exchangerate-api.com/v6/34eec863ffdfe1945f8c0f1a/latest/USD")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            var json = JObject.Parse(response.Content);
            var Currency = json["conversion_rates"]["IQD"].ToString();
            if (ToUSD == true)
            {
                client = new RestClient("https://v6.exchangerate-api.com/v6/34eec863ffdfe1945f8c0f1a/latest/IQD")
                {
                    Timeout = -1
                };
                request = new RestRequest(Method.GET);
                response = client.Execute(request);
                json = JObject.Parse(response.Content);
                Currency = json["conversion_rates"]["USD"].ToString();
            }
            return Amount * Double.Parse(Currency);
        }
        public async Task<double> Convert(Guid CompantId, double Amount, bool ToUSD)
        {
            var company = await _repositoryWrapper.Company.FindById(CompantId);
            if (company.IsAcceptAutomaticCurrencyExchange)
            {
                return CurrencyConverting(Amount, ToUSD);
            }
            else
            {
                if (ToUSD) return Amount / company.ExchangeRate;
                else return Amount * company.ExchangeRate;
            }
        }
    }
}