using AutoMapper;
using DAL;
using DAL.Services;
using Repository;
using System;

namespace RepresentativesTracking
{
    public interface IRepositoryWrapper : IDisposable
    {
        ICompanyRepository Company { get; }
        IUsersRepository User { get; }
        IOrderRepository Order { get; }
        ILocationRepository Location { get; }
        void Save();
    }

    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DBContext _repoContext;
        private ICompanyRepository _company;
        private IUsersRepository _user;
        private IOrderRepository _order;
        private ILocationRepository _location;
        public ICompanyRepository Company => _company ??= new CompanyRepository(_repoContext);
        public IUsersRepository User => _user ??= new UserRepository(_repoContext);
        public IOrderRepository Order => _order ??= new OrderRepository(_repoContext);
        public ILocationRepository Location => _location ??= new LocationRepository(_repoContext);
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
