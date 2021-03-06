using DAL;
using DAL.Services;
using Repository;
using System;
namespace RepresentativesTracking
{
    public interface IRepositoryWrapper : IDisposable
    {
        ICompanyRepository Company { get; }
        IProductsRepository Products { get; }
        IUsersRepository User { get; }
        IOrderRepository Order { get; }
        ILocationRepository Location { get; }
        ICustomerRepository Customer { get; }

        void Save();
    }
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DBContext _repoContext;
        private ICompanyRepository _company;
        private IUsersRepository _user;
        private IOrderRepository _order;
        private ILocationRepository _location;
        private IProductsRepository _products;
        private ICustomerRepository _customer;
        public ICustomerRepository Customer => _customer ??= new CustomerRepository(_repoContext);
        public ICompanyRepository Company => _company ??= new CompanyRepository(_repoContext);
        public IUsersRepository User => _user ??= new UserRepository(_repoContext);
        public IOrderRepository Order => _order ??= new OrderRepository(_repoContext);
        public ILocationRepository Location => _location ??= new LocationRepository(_repoContext);
        public IProductsRepository Products => _products ??= new ProductsRepository(_repoContext);
        public RepositoryWrapper(DBContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }
        public void Save()
        {
            _repoContext.SaveChanges();
        }
        public void Dispose()
        {
            _repoContext.Dispose();
        }
    }
}