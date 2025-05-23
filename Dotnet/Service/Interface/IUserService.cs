using Service.Dto.Request;
using Service.Helper;

namespace Service.Interface
{
    public interface IUserService
    {
        Task<ApiResponse> Create(CreateUserRequest dto);
        Task<ApiResponse> Login(LoginRequest dto);
    }
}