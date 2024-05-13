namespace EProject_Sem_3.Models;

public class VnPaymentSubscriptionRequestModel
{
    
    public decimal TotalAmount { get; set; }
    
    public int UserId { get; set; }
    
    public int PlanId { get; set; }
}