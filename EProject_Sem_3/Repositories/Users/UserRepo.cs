using AutoMapper;
using BCrypt.Net;
using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Subscriptions;
using EProject_Sem_3.Models.Users;
using EProject_Sem_3.Services.FileService;
using EProject_Sem_3.Services.TokenService;
using Microsoft.EntityFrameworkCore;

namespace EProject_Sem_3.Repositories.Users;

public class UserRepo : IUserRepo {

    private readonly AppDbContext context;

    private readonly ITokenService tokenService;

    private readonly IMapper mapper;

    private readonly IFileService fileService;

    public UserRepo(AppDbContext context, ITokenService tokenService, IMapper mapper, IFileService fileService) {
        this.context = context;
        this.tokenService = tokenService;
        this.mapper = mapper;
        this.fileService = fileService;
    }

    public async Task<User> Register(RegisterDto dto) {

        // check if plan exist
        if (!(await context.Plans.AnyAsync(p => p.Id == dto.PlanId))) throw new NotFoundException("Plan not found");

        //Map user
        User newUser = mapper.Map<User>(dto);
        newUser.Role = Role.User;
        newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

        // create new user
        var entityEntry = await context.Users.AddAsync(newUser);
        var entity = entityEntry.Entity;

        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<TokenDto> Login(LoginDto dto) {
        var user = await GetUserDetail(dto.Username);
        
        //check activation 
        if (!user.IsActivated)
        {
            throw new UnauthorizedAccessException("User is not activated");
        }
        
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) {
            throw new BadRequestException("Password incorrect");
        }
        var token = tokenService.CreateToken(user);

        return new TokenDto {
            User = mapper.Map<UserRes>(user),
            AccessToken = token
        };
    }

    public async Task<User> FindByUsername(string username) {
        var user = await context.Users.FirstOrDefaultAsync(e => e.Username.Equals(username));
        return user ?? throw new NotFoundException("User not found");
    }

    public async Task<User> GetUserDetail(string username) {
        return await context.Users
            .Where(u => u.Username == username)
            .Include(u => u.Subscriptions)
            .FirstOrDefaultAsync() ?? throw new NotFoundException("User not found");
    }

    public async Task<PaginationRes<User>> FindAll(PaginationReq pageReq) {
        
        var user = await context.Users
            .OrderBy(u => u.Id)
            .Skip((pageReq.PageNo - 1) * pageReq.PerPage)
            .Take(pageReq.PerPage)
            .ToListAsync();

        var totalRecords = await context.Users.CountAsync();

        return new PaginationRes<User>(pageReq.PageNo, pageReq.PerPage, totalRecords, user);
    }

    public async Task<object?> FindById(int id) {
        var user = await context.Users
            .Where(u => u.Id == id)
            .Include(u => u.Subscriptions)
            .ThenInclude(s => s.Plan)
            .FirstOrDefaultAsync();
        return user == null ? throw new NotFoundException("User not found") : (object)mapper.Map<UserRes>(user);
    }

    public async Task<User> UpdateUser(User user, UserUpdateDto dto) {
        mapper.Map(dto, user);
        await context.SaveChangesAsync();
        return user;
    }


    public async Task ChangePassword(User user, ChangePasswordDto dto) {
        user.Password = BCrypt.Net.BCrypt
            .ValidateAndReplacePassword(
                dto.OldPassword,
                user.Password,
                dto.NewPassword
            );
        await context.SaveChangesAsync();
    }

    public async Task ActivateUser(int userId)
    {
        // find user with userId
        var userCurrent = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (userCurrent == null)
        {
            throw new NotFoundException("User not found");
        }
        userCurrent.IsActivated = true;
        await context.SaveChangesAsync();

    }

    public async Task<string> UpdateAvatar(User user, IFormFile avatar) {
        var fileModel = new FileModel() {
            Path = "users", 
            Name = user.Username, 
            File = avatar
        };
        var url = await fileService.Upload(fileModel);
        user.Avatar = url;
        await context.SaveChangesAsync();
        return url;
    }

    public async Task DeleteAvatar(User user) {
        user.Avatar = null;
        await fileService.Delete("users/" + user.Username);
        await context.SaveChangesAsync();
    }
}