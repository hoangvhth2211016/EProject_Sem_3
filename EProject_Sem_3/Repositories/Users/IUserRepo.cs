using EProject_Sem_3.Models.Users;

namespace EProject_Sem_3.Repositories.Users;

public interface IUserRepo {

    Task Register(RegisterDto dto);

    Task<TokenDto> Login(LoginDto dto);

    Task<User> FindByUsername(string username);

    Task<List<User>> FindAll();

    Task<object?> FindById(int id);

    Task<User> UpdateUser(User user, UserUpdateDto dto);

    Task ChangePassword(User user, ChangePasswordDto dto);

    Task ActivateUser(int userId);
}