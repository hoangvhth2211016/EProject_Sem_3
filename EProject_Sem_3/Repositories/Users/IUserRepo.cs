using EProject_Sem_3.Models.Users;

namespace EProject_Sem_3.Repositories.Users;

public interface IUserRepo {
    Task<int> Register(User user);

    Task<TokenDto> Login(LoginDto dto);

    Task<User> FindByUsername(string username);

}