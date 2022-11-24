using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Check.Interfaces;
using Check.Options;
using Check.Models;
using Check.Database;
using System.Text;
using AutoMapper;
using Check.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Check.Services;

public class SessionService : ISessionService
{
    private readonly JwtOptions _jwtOptions;
    private readonly AppDbContext _appDbContext;

    public SessionService(IOptions<JwtOptions> jwtOptions, AppDbContext appDbContext)
    {
        _jwtOptions = jwtOptions.Value;
        _appDbContext = appDbContext;   
    }

    public async Task<TokenVm> SignInAsync(SignInModel model)
    {
        var passwordHash = GetPasswordHash(model.Password);
        User? user = await _appDbContext.Users.FirstOrDefaultAsync(u => 
            u.Username == model.Username 
            && u.Password == passwordHash
        );

        if (user is null) 
            throw new Exception("Incorrect username or password");

        var accessClaims = new List<Claim> { 
            new Claim(JwtRegisteredClaimNames.UniqueName, model.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("guid", user.Id.ToString()),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
        };
        var refreshClaims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
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

    public string GetPasswordHash(string password)
    {
        byte[] hash;
        using (HashAlgorithm algorithm = SHA256.Create())
            hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));

        StringBuilder sb = new StringBuilder();
        foreach (byte b in hash)
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
}
