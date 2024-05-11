using EProject_Sem_3.Exceptions;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Plans;
using Microsoft.EntityFrameworkCore;

namespace EProject_Sem_3.Repositories.Plans {
    public class PlanRepo : IPlanRepo {

        private readonly AppDbContext context;

        public PlanRepo(AppDbContext context) {
            this.context = context;
        }

        public async Task<List<Plan>> FindAll() {
            return await context.Plans.ToListAsync();
        }

        public async Task<Plan> FindById(int id) {
            return await context.Plans.FirstOrDefaultAsync(p => p.Id == id) ?? throw new NotFoundException("Plan not found");
        }
    }
}
