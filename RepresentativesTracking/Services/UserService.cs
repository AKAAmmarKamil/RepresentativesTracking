using Model.Form;
using Modle.Model;
using RepresentativesTracking;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService : IBaseService<User, Guid>
    {
        Task<User> Authintication(LoginForm login);
        Task<User> GetUserByEmail(string Email);
        Task<bool> ChangePassword(Guid Id, string Password);
        Task<bool> ChangeStatus(Guid Id, bool Status);
        Task<IEnumerable<User>> FindAll(int PageNumber,int Count);
        Task<IEnumerable<User>> GetUsersByCompany(Guid Id, int PageNumber,int Count);
        Task<User> ModifyByAdmin(Guid id, User User);
    }

    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public UserService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IEnumerable<User>> All(int PageNumber,int Count) => await _repositoryWrapper.User.FindAll(PageNumber, Count);
        public async Task<IEnumerable<User>> GetUsersByCompany(Guid Id,int PageNumber,int Count) => await _repositoryWrapper.User.GetUsersByCompany(Id,PageNumber, Count);

        public Task<User> Authintication(LoginForm login) =>
            _repositoryWrapper.User.Authintication(login);
        public async Task<bool> ChangePassword(Guid Id, string Password)
        {
            var UserModelFromRepo = await _repositoryWrapper.User.FindById(Id);
            if (UserModelFromRepo == null)
            {
                return false;
            }
            UserModelFromRepo.Password = Password;
            _repositoryWrapper.Save();
            return true;
        }
        public async Task<bool> ChangeStatus(Guid Id, bool Status)
        {
            var UserModelFromRepo = await _repositoryWrapper.User.FindById(Id);
            if (UserModelFromRepo == null)
            {
                return false;
            }
            UserModelFromRepo.Activated = Status;
            _repositoryWrapper.Save();
            return true;
        }
        public async Task<User> Create(User User) => await
             _repositoryWrapper.User.Create(User);

        public async Task<User> Delete(Guid id) => await
        _repositoryWrapper.User.Delete(id);

        public Task<IEnumerable<User>> FindAll(int PageNumber,int Count) => _repositoryWrapper.User.FindAll(PageNumber,Count);

        public async Task<User> FindById(Guid id) => await
        _repositoryWrapper.User.FindById(id);

        public Task<User> GetUserByEmail(string Email) =>
        _repositoryWrapper.User.GetUserByEmail(Email);

        public async Task<User> ModifyByAdmin(Guid id, User User)
        {
            var UserModelFromRepo = await _repositoryWrapper.User.FindById(id);
            if (UserModelFromRepo == null)
            {
                return null;
            }
            UserModelFromRepo.UserName = User.UserName;
            UserModelFromRepo.Email = User.Email;
            UserModelFromRepo.PhoneNumber = User.PhoneNumber;
            UserModelFromRepo.Type = User.Type;
            _repositoryWrapper.Save();
            return User;
        }
        public async Task<User> Modify(Guid id, User User)
        {
            var UserModelFromRepo = await _repositoryWrapper.User.FindById(id);
            if (UserModelFromRepo == null)
            {
                return null;
            }
            UserModelFromRepo.UserName = User.UserName;
            UserModelFromRepo.Email = User.Email;
            UserModelFromRepo.PhoneNumber = User.PhoneNumber;
            _repositoryWrapper.Save();
            return User;
        }
    }
}