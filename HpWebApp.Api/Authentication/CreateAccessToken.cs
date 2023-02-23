using Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HpWebApp.Api.Authentication;

public interface ICreateAccessToken
{
    string CreateToken(User user);
}

public class CreateAccessToken : ICreateAccessToken
{
    private readonly IConfiguration _config;

    public CreateAccessToken(IConfiguration config)
    {
        _config = config;
    }

    public string CreateToken(User user)
    {
        // Create Claims.
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Name!.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!)
        };

        var skey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(skey, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
