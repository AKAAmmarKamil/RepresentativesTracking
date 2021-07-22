using Modle.Model;
using RepresentativesTracking;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Services
{
    public interface IProductsService : IBaseService<Products, int>
    {
        Task<List<Products>> GetAll(int PageNumber, int Count);
        Task<Products> FindById(int Id);
        Task<List<Products>> GetAllByOrder(int OrderId, int PageNumber, int Count);
    }
    public class ProductsService : IProductsService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public ProductsService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public Task<IEnumerable<Products>> All(int PageNumber, int Count)=>_repositoryWrapper.Products.FindAll(PageNumber, Count);
        public async Task<Products> Create(Products Products) => await
             _repositoryWrapper.Products.Create(Products);
        public async Task<Products> Delete(int id) => await
        _repositoryWrapper.Products.Delete(id);
        public async Task<Products> FindById(int id) => await
        _repositoryWrapper.Products.FindById(id);
        public async Task<List<Products>> GetAll(int PageNumber, int Count) =>await _repositoryWrapper.Products.GetAll(PageNumber,Count);
        public async Task<List<Products>> GetAllByOrder(int OrderId, int PageNumber, int Count) => await _repositoryWrapper.Products.GetAllByOrder(OrderId,PageNumber,Count);
        public async Task<Products> Modify(int id, Products Products)
        {
            var ProductsModelFromRepo = await _repositoryWrapper.Products.FindById(id);
            if (ProductsModelFromRepo == null)
            {
                return null;
            }
            ProductsModelFromRepo.Name = Products.Name;
            ProductsModelFromRepo.Count = Products.Count;
            ProductsModelFromRepo.PriceInIQD = Products.PriceInIQD;
            ProductsModelFromRepo.PriceInUSD = Products.PriceInUSD;
            ProductsModelFromRepo.OrderID = Products.OrderID;
            _repositoryWrapper.Save();
            return Products;
        }
    }
}