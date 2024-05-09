using AutoMapper;
using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Feedbacks;
using Microsoft.EntityFrameworkCore;

namespace EProject_Sem_3.Repositories.Feedbacks {
    public class FeedbackRepo : IFeedbackRepo {

        private readonly AppDbContext context;

        private readonly IMapper mapper;

        public FeedbackRepo(AppDbContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task Create(FeedbackCreateDto dto) {
            var newFeedback = mapper.Map<Feedback>(dto);
            await context.Feedbacks.AddAsync(newFeedback);
            await context.SaveChangesAsync();
        }

        public async Task<ICollection<Feedback>> FindAll() {
            return await context.Feedbacks.ToListAsync();
        }

        public async Task<Feedback> FindByEmail(string email) {
            return await context.Feedbacks.FirstOrDefaultAsync(fb => fb.Email == email) ?? throw new NotFoundException("Feedback not found");
        }

        public async Task<Feedback> FindById(int id) {
            return await context.Feedbacks.FirstOrDefaultAsync(fb => fb.Id == id) ?? throw new NotFoundException("Feedback not found");
        }
    }
}
