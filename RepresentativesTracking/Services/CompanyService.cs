using Modle.Model;
using RepresentativesTracking;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Services
{
    public interface ICompanyService : IBaseService<Company, int>
    {
        Task<IEnumerable<Company>> GetAll();
        Task<Company> ModifyExchange(int id, Company Company);
    }
    public class CompanyService : ICompanyService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public CompanyService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public Task<IEnumerable<Company>> All(int PageNumber, int Count)=>_repositoryWrapper.Company.FindAll(PageNumber, Count);
        public async Task<Company> Create(Company Company) => await
             _repositoryWrapper.Company.Create(Company);
        public async Task<Company> Delete(int id) => await
        _repositoryWrapper.Company.Delete(id);

        public async Task<Company> FindById(int id) => await
        _repositoryWrapper.Company.FindById(id);

        public async Task<IEnumerable<Company>> GetAll()=>await _repositoryWrapper.Company.GetAll();

        public async Task<Company> Modify(int id, Company Company)
        {
            var CompanyModelFromRepo = await _repositoryWrapper.Company.FindById(id);
            if (CompanyModelFromRepo == null)
            {
                return null;
            }
            CompanyModelFromRepo.Name = Company.Name;
            CompanyModelFromRepo.RepresentativeCount = Company.RepresentativeCount;
            _repositoryWrapper.Save();
            return Company;
        }
        public async Task<Company> ModifyExchange(int id, Company Company)
        {
            var CompanyModelFromRepo = await _repositoryWrapper.Company.FindById(id);
            if (CompanyModelFromRepo == null)
            {
                return null;
            }
            CompanyModelFromRepo.ExchangeRate = Company.ExchangeRate;
            CompanyModelFromRepo.IsAcceptAutomaticCurrencyExchange = Company.IsAcceptAutomaticCurrencyExchange;
            _repositoryWrapper.Save();
            return Company;
        }
      
    }
}