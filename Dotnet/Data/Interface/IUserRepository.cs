using Model.Entities;

namespace Data.Interface
{
    public interface IUserRepository
    {
        Task<Users?> Create(Users user);
        Task<Users?> GetByEmail(string email);
        Task<Users?> GetByEmailOrPhoneAndPassword(string emailOrPhone, string password);
    }
}