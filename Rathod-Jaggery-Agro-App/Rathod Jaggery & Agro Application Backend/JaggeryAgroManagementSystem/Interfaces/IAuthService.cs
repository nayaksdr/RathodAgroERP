using JaggeryAgroManagementSystem.DTOs;

namespace JaggeryAgroManagementSystem.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(UserRegisterDto dto);
        Task<string> Login(UserLoginDto dto);
    }

}
