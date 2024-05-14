namespace EProject_Sem_3.Models.Users;

public class TokenDto
{

    public required UserRes User { get; set; }

    public required string AccessToken { get; set; }
    
}