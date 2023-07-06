using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRO_APBD.Server.Services;

public interface ITokenService
{
    string GenerateToken(string username);
}
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = new JwtSecurityToken
        (
         issuer: _configuration["JWT:Issuer"],
        audience: _configuration["JWT:Audience"],
        expires: DateTime.Now.AddDays(1),
        signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!)),
                    SecurityAlgorithms.HmacSha256
            ),
        claims: new List<Claim> { new Claim(ClaimTypes.Name, username) }
        );


        var stringifiedToken = tokenHandler.WriteToken(token);
        return stringifiedToken;
    }

}
