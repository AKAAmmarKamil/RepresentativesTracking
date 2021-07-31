using Modle.Model;
using RepresentativesTracking;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Services
{
    public interface ICustomerService : IBaseService<Customer, int>
    {
        Task<Customer> GetById(int Id,int CompanyId);
        Task<IEnumerable<Customer>> GetAll();
        Task<IEnumerable<Customer>> GetCustomersByCompany(int CompanyId, int PageNumber, int Count);
    }
    public class CustomerService : ICustomerService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public CustomerService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public Task<IEnumerable<Customer>> All(int PageNumber, int Count)=>_repositoryWrapper.Customer.FindAll(PageNumber, Count);
        public Task<IEnumerable<Customer>> GetCustomersByCompany(int CompanyId,int PageNumber, int Count) => _repositoryWrapper.Customer.GetCustomersByCompany(CompanyId,PageNumber, Count);
        public async Task<Customer> Create(Customer Customer) => await
             _repositoryWrapper.Customer.Create(Customer);
        public async Task<Customer> Delete(int id) => await
        _repositoryWrapper.Customer.Delete(id);

        public async Task<Customer> GetById(int id, int CompanyId) => await
        _repositoryWrapper.Customer.GetById(id,CompanyId);

        public async Task<IEnumerable<Customer>> GetAll()=>await _repositoryWrapper.Customer.GetAll();

        public async Task<Customer> Modify(int id, Customer Customer)
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

        public Task<Customer> FindById(int id) => _repositoryWrapper.Customer.FindById(id);
    }
}