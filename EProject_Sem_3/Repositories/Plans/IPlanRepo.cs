using EProject_Sem_3.Models.Plans;

namespace EProject_Sem_3.Repositories.Plans {
    public interface IPlanRepo {

        Task<Plan> FindById(int id);

        Task<List<Plan>> FindAll();

    }
}
