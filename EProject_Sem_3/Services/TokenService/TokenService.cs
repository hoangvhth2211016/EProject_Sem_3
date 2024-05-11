using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EProject_Sem_3.Models.Users;
using Microsoft.IdentityModel.Tokens;

namespace EProject_Sem_3.Services.TokenService;

public class TokenService : ITokenService {

    private const int Expire = 1;

    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration) {
        _configuration = configuration;
    }

    public string CreateToken(User user) {
        var expiration = DateTime.UtcNow.AddDays(Expire);

        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSigningCredentials(),
            expiration
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration) {
        return new(
            null,
            null,
            claims,
            expires: expiration,
            signingCredentials: credentials
        );
    }

    private List<Claim> CreateClaims(User user) {
        // var jwtSub = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];

        var jwtSub = _configuration.GetSection("Jwt")["JwtRegisteredClaimNamesSub"];

        try {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtSub),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim("Email", user.Email),
                new Claim("Id", user.Id.ToString()),
                new Claim("Name", user.Username),
                new Claim("Role", user.Role.ToString()),
            };

            return claims;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }

    private SigningCredentials CreateSigningCredentials() {
        // var symmetricSecurityKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"];

        var symmetricSecurityKey = _configuration.GetSection("Jwt")["SymmetricSecurityKey"];

        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey)
            ),
            SecurityAlgorithms.HmacSha256
        );
    }

}