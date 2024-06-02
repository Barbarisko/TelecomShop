using Npgsql;
using TelecomShop.DBModels;
using TelecomShop.ErrorHandlers;
using TelecomShop.UnitOfWork;

namespace TelecomShop.Services
{
    public interface IAuthService
    {
        User GetUserDataWithAuth(string phoneNumber, string password);
        public User AddUser(string phoneNumber, string password, string name, string surname);
    }
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        public AuthService(IUnitOfWork _unitOfWork) {
            unitOfWork = _unitOfWork;
        }

        public User GetUserDataWithAuth(string phoneNumber, string password)
        {
            var allUsers = unitOfWork.UserRepo.GetAll();
            var userByPhone = allUsers.First((user) => user.Msisdn == phoneNumber);
            if(userByPhone == null)
            {
                throw new BadRequestException("Number or Password are incorrect!");
            }

            if (password != userByPhone.Password)
                throw new BadRequestException("Number or Password are incorrect!");

            return userByPhone;
        }

        public User GetUserData(int id)
        {
            return unitOfWork.UserRepo.Get(id);
        }

        public User AddUser(string phoneNumber, string password, string name, string surname)
        {
            return unitOfWork.UserRepo.Add(new User
            {
                Msisdn = phoneNumber,
                Password = password,
                Name = name,
                Surname = surname
            });
        }
    }
}
