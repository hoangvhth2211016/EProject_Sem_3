namespace EProject_Sem_3.Models;

public class VnPaymentRequestModel
{
    public int? OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Phone { get; set; }
    
    public string? UserId { get; set; }
    
    public string? PlanId { get; set; }
}