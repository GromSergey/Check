using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Check.Interfaces;
using Check.Options;
using Check.Models;

namespace Check.Services;

public class SessionService : ISessionService
{
    private readonly JwtOptions _jwtOptions;

    public SessionService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<TokenVm> SignInAsync(SignInModel model)
    {
        var sid = "TODO";
        var email = "TODO";
        var id = "TODO";

        var accessClaims = new List<Claim> { 
            new Claim(JwtRegisteredClaimNames.UniqueName, model.Username),
            new Claim(JwtRegisteredClaimNames.Sid, sid),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.NameId, id)
        };
        var refreshClaims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sid, sid),
            new Claim(JwtRegisteredClaimNames.NameId, id)
        };
        
        var tokens = new TokenVm
        {
            AccessToken = GenerateToken(accessClaims, _jwtOptions.GetAccessLifetime()),
            RefreshToken = GenerateToken(refreshClaims, _jwtOptions.GetRefreshLifetime())
        };
        return tokens;
    }
    public string GenerateToken(IEnumerable<Claim> claims, TimeSpan lifetime)
    {
        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(lifetime),
            signingCredentials: new SigningCredentials(_jwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }
}
