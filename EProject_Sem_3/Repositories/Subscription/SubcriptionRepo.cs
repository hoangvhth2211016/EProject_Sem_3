using AutoMapper;
using EProject_Sem_3.Models;
using EProject_Sem_3.Models.Subscriptions;

namespace EProject_Sem_3.Repositories;

public class SubscriptionRepo : ISubscriptionRepo
{
    private readonly AppDbContext _context;
    

    public SubscriptionRepo(AppDbContext context)
    {
        _context = context;
    }


    public async Task CreateSubscription(int userId, int planId)
    {
        
        var newSub = new Subscription();
        newSub.UserId = userId;
        newSub.PlanId = planId;
        
        if (planId == 1)
        {
            // Nếu planId là 1, thêm 1 tháng vào thời gian hiện tại
            newSub.ExpiredAt = DateTime.Now.AddMonths(1);
        }
        else if (planId == 2)
        {
            // Nếu planId là 2, thêm 1 năm vào thời gian hiện tại
            newSub.ExpiredAt = DateTime.Now.AddYears(1);
        }
        
        // save to database
        _context.Subscriptions.Add(newSub);
        await _context.SaveChangesAsync();
    }
}