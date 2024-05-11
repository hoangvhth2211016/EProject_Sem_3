using EProject_Sem_3.Models.Feedbacks;
using EProject_Sem_3.Repositories.Feedbacks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EProject_Sem_3.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBacksController : ControllerBase {

        private readonly IFeedbackRepo feedbackRepo;

        public FeedBacksController(IFeedbackRepo feedbackRepo) {
            this.feedbackRepo = feedbackRepo;
        }

        /// <summary>
        /// get all feedbacks, only for admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            return Ok(await feedbackRepo.FindAll());
        }

        /// <summary>
        /// get feedback by id, only for admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) {
            return Ok(await feedbackRepo.FindById(id));
        }

        /// <summary>
        /// create feedback
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(FeedbackCreateDto dto) {
            await feedbackRepo.Create(dto);
            return Ok("Feedback created");
        }

    }
}
