namespace EProject_Sem_3.Repositories;

public interface ISubscriptionRepo
{
    Task CreateSubscription(int userId, int planId);
}