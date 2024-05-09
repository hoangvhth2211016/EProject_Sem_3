using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Users;
using EProject_Sem_3.Services.TokenService;
using Microsoft.EntityFrameworkCore;

namespace EProject_Sem_3.Repositories.Users;

public class UserRepo : IUserRepo {

    private AppDbContext context;

    private ITokenService tokenService;

    public UserRepo(AppDbContext context, ITokenService tokenService) {
        this.context = context;
        this.tokenService = tokenService;
    }

    public async Task<int> Register(User user) {
        await context.Users.AddAsync(user);
        return await context.SaveChangesAsync();
    }

    public async Task<TokenDto> Login(LoginDto dto) {
        var user = await FindByUsername(dto.Username);
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) {
            throw new BadRequestException("Password incorrect");
        }
        var token = tokenService.CreateToken(user);

        return new TokenDto { AccessToken = token };
    }

    public async Task<User> FindByUsername(string username) {
        var user = await context.Users.FirstOrDefaultAsync(e => e.Username.Equals(username));
        if (user == null) {
            throw new NotFoundException("User not found");
        }
        return user;
    }
}