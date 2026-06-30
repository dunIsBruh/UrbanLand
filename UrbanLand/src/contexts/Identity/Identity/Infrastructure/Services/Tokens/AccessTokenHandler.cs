using System.IdentityModel.Tokens.Jwt;
using Identity.Infrastructure.Options;

namespace Identity.Infrastructure.Services.Tokens;

public static class AccessTokenHandler
{
    public static string Write(IEnumerable<Claim> claims, JwtSettings jwtsSettings)
	{
		var token = Generate(claims, jwtsSettings);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	private static JwtSecurityToken Generate(IEnumerable<Claim> claims, JwtSettings jwtsSettings)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsSettings.SecretKey));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		
		var token = new JwtSecurityToken(
			issuer: jwtsSettings.Issuer,
			audience: jwtsSettings.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(jwtsSettings.AccessTokenExpirationMinutes),
			signingCredentials: credentials);

		return token;
	}
}