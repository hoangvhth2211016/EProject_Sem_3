using EProject_Sem_3.Repositories.Plans;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase {

        private readonly IPlanRepo planRepo;

        public PlansController(IPlanRepo planRepo) {
            this.planRepo = planRepo;
        }

        /// <summary>
        /// get all subscription plan
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            return Ok(await planRepo.FindAll());
        }

        /// <summary>
        /// get plan by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) {
            return Ok(await planRepo.FindById(id));
        }
    }
}
