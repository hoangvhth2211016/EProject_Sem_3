using AutoMapper;
using EProject_Sem_3.Models.Users;
using EProject_Sem_3.Repositories.Plans;
using EProject_Sem_3.Repositories.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly IUserRepo userRepo;


        public AuthController(IUserRepo userRepo) {
            this.userRepo = userRepo;
        }

        /// <summary>
        /// register user
        /// </summary>
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto) {
            await userRepo.Register(dto);
            return Ok("new user created");
        }

        /// <summary>
        /// login
        /// </summary>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto) {
            TokenDto token = await userRepo.Login(dto);
            return Ok(token);
        }

        //[Authorize(Roles = "User,Admin")]
        [HttpGet("Test")]
        public IActionResult Test() {
            return Ok(User.Identity.IsAuthenticated);
        }

    }
}
