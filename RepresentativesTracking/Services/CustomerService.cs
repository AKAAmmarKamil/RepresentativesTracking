using Modle.Model;
using RepresentativesTracking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Services
{
    public interface ICustomerService : IBaseService<Customer, Guid>
    {
        Task<Customer> GetById(Guid Id,Guid CompanyId);
        Task<IEnumerable<Customer>> GetAll();
        Task<IEnumerable<Customer>> GetCustomersByCompany(Guid CompanyId, int PageNumber,int Count);
    }
    public class CustomerService : ICustomerService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public CustomerService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public Task<IEnumerable<Customer>> All(int PageNumber,int Count)=>_repositoryWrapper.Customer.FindAll(PageNumber, Count);
        public Task<IEnumerable<Customer>> GetCustomersByCompany(Guid CompanyId,int PageNumber,int Count) => _repositoryWrapper.Customer.GetCustomersByCompany(CompanyId,PageNumber, Count);
        public async Task<Customer> Create(Customer Customer) => await
             _repositoryWrapper.Customer.Create(Customer);
        public async Task<Customer> Delete(Guid id) => await
        _repositoryWrapper.Customer.Delete(id);

        public async Task<Customer> GetById(Guid id, Guid CompanyId) => await
        _repositoryWrapper.Customer.GetById(id,CompanyId);

        public async Task<IEnumerable<Customer>> GetAll()=>await _repositoryWrapper.Customer.GetAll();

        public async Task<Customer> Modify(Guid id, Customer Customer)
        {
            var CustomerModelFromRepo = await _repositoryWrapper.Customer.FindById(id);
            if (CustomerModelFromRepo == null)
            {
                return null;
            }
            CustomerModelFromRepo.FullName = Customer.FullName;
            CustomerModelFromRepo.PhoneNumber = Customer.PhoneNumber;
            _repositoryWrapper.Save();
            return Customer;
        }

        public Task<Customer> FindById(Guid id) => _repositoryWrapper.Customer.FindById(id);
    }
}