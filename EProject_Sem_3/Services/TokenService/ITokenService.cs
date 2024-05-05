using EProject_Sem_3.Models.Users;

namespace EProject_Sem_3.Services.TokenService;

public interface ITokenService
{
    string CreateToken(User user);
}