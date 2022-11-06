using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Check.Options;

public class JwtOptions
{
    public string Secret { get; set; } = string.Empty;
    public string AccessLifetime { get; set; } = string.Empty;
    public string RefreshLifetime { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }

    public TimeSpan GetAccessLifetime() =>
        TimeSpan.TryParse(AccessLifetime, out var result) ? result : TimeSpan.FromMinutes(5);

    public TimeSpan GetRefreshLifetime() =>
        TimeSpan.TryParse(RefreshLifetime, out var result) ? result : TimeSpan.FromDays(14);
}
