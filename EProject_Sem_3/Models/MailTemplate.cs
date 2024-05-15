using System.ComponentModel.DataAnnotations;

namespace EProject_Sem_3.Models;

public class MailTemplate
{
    [EmailAddress]
    public required string ToAddress { get; set; }
    
    public required string Subject { get; set; }
    
    public required string Body { get; set; }
}