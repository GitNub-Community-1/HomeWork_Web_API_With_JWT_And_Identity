using Domain.Dtos;
using WebAPIWithJWTAndIdentity.Response;

namespace Infrastructure.Services;

public interface IAccountService
{
    Task<Response<RegisterDto>> Register(RegisterDto model);
    Task<Response<string>> Login(LoginDto login);
    Task<Response<string>> AddOrRemoveUserFromRole(UserRoleDto userRole, bool delete = false);
}