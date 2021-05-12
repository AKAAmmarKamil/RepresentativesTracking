using Modle.Model;
using RepresentativesTracking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Services
{
    public interface ICompanyService : IBaseService<Company, int>
    {
        Task<IEnumerable<Company>> GetAll();
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
            var CountryModelFromRepo = await _repositoryWrapper.Company.FindById(id);
            if (CountryModelFromRepo == null)
            {
                return null;
            }
            CountryModelFromRepo.Name = Company.Name;
            _repositoryWrapper.Save();
            return Company;
        }

    }
}