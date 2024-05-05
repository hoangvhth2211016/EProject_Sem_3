using AutoMapper;
using EProject_Sem_3.Models.Users;
using EProject_Sem_3.Repositories.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly IUserRepo userRepo;

        private IMapper mapper;

        public AuthController(IUserRepo userRepo, IMapper mapper) {
            this.userRepo = userRepo;
            this.mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            User newUser = mapper.Map<User>(dto);
            newUser.Role = Role.User;
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            int success = await userRepo.Register(newUser);
            if (success > 0) {
                return Ok(newUser);
            }
            return BadRequest("Unable to create user");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            TokenDto token = await userRepo.Login(dto);
            if (token == null) {
                return BadRequest("Unable to login");
            }
            return Ok(token);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("Test")]
        public IActionResult Test() {
            return Ok("this route is protected");
        }

    }
}
