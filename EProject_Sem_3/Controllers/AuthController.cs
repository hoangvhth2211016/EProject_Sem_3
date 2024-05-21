using AutoMapper;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Users;
using EProject_Sem_3.Repositories.Books;
using EProject_Sem_3.Repositories.Plans;
using EProject_Sem_3.Repositories.Users;
using EProject_Sem_3.Services.VnpayService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace EProject_Sem_3.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly IUserRepo userRepo;
        
        private readonly IVnPayService vnPayService;
        
        private readonly IPlanRepo planRepo;

        public AuthController(IUserRepo userRepo,IVnPayService vnPayService,IPlanRepo planRepo) {
            this.userRepo = userRepo;
            this.vnPayService = vnPayService;
            this.planRepo = planRepo;

        }

        /// <summary>
        /// register user
        /// </summary>
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto) {
            var user = await userRepo.Register(dto);
            var model = new VnPaymentSubscriptionRequestModel();
            
            // var plan = await planRepo.FindById(dto.planId);
            model.TotalAmount = (dto.PlanId == 1 ? 15 : 150) * 25000; 
            model.PlanId = dto.PlanId;
            model.UserId = user.Id;
            
            
            return Ok(vnPayService.CreatePaymentUrlForSubscription(model));
                
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
