using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SecretariaIa.Common.DTOs;

public static class TokenUtil
{
	public static TokenDTO CreateToken(UserTokenDTO user, TokenConfigDTO config)
	{
		List<Claim> claims =
		[
			new(ClaimTypes.NameIdentifier, user.Id),
				new(ClaimTypes.Role, user.Role.ToString()),
				new(ClaimTypes.Email, user.Email ?? string.Empty),
				new(ClaimTypes.Name, user.Name ?? string.Empty),
			];

		var expiresAt = DateTime.UtcNow.AddMinutes(config.MinutesToExpiresToken);

		(var issuer, var audience) = Descriptor(config, false);

		string token = GenerateToken
		(
			secretKey: config.SecretKey,
			identifier: user.Id,
			expirationAt: expiresAt,
			issuer: issuer,
			audience: audience,
			additionsClaims: claims
		);
		string refreshToken = GenerateRefreshToken(user.Id, config);

		return new(token, refreshToken, expiresAt.AddHours(-3));
	}
	public static async Task<TokenDTO> CreateTokenAsync(Func<Task<UserTokenDTO>> getUser, TokenConfigDTO config)
	{
		var user = await getUser();
		return CreateToken(user, config);
	}
	public static async Task<TokenDTO?> RefreshTokenAsync(Func<string, Task<UserTokenDTO>> getUser, string refreshToken, TokenConfigDTO config)
	{
		JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();
		(string, string) tuple = Descriptor(config, refreshToken: true);
		string issuer = tuple.Item1;
		string audience = tuple.Item2;
		TokenValidationResult token = await tokenHandler.ValidateTokenAsync(refreshToken, new TokenValidationParameters
		{
			ValidIssuer = issuer,
			ValidAudience = audience,
			IssuerSigningKey = CreateKey(config.SecretKey)
		});
		if (!token.IsValid)
		{
			return null;
		}
		string identificador = token.ClaimsIdentity.Claims.First((Claim c) => c.Type == "sub").Value;
		UserTokenDTO user = await getUser(identificador);
		ArgumentNullException.ThrowIfNull(user, "user");
		return CreateToken(user, config);
	}

	private static string GenerateToken(string secretKey, string identifier, DateTime expirationAt, string issuer, string audience, IEnumerable<Claim>? additionsClaims = null)
	{
		JwtSecurityTokenHandler tokenHandler = new();

		var claims = new ClaimsIdentity(
		[
			new Claim(ClaimTypes.NameIdentifier, identifier)
		]);

		if (additionsClaims is not null)
		{
			foreach (var item in additionsClaims)
			{
				if (!claims.HasClaim(c => c.Type == item.Type))
					claims.AddClaim(item);
			}
		}
		SecurityTokenDescriptor tokenDescriptor = new()
		{
			Subject = claims,
			Expires = expirationAt,
			Issuer = issuer,
			Audience = audience,
			SigningCredentials = CreateSigning(secretKey)
		};
		return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
	}
	private static SigningCredentials CreateSigning(string secretKey)
	{
		return new
		(
			key: CreateKey(secretKey),
			algorithm: SecurityAlgorithms.HmacSha256Signature
		);
	}
	private static string GenerateRefreshToken(string sub, TokenConfigDTO config, string tipo = "rt-jwt")
	{
		ClaimsIdentity claims = new([new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, sub)]);

		(var issuer, var audience) = Descriptor(config, true);

		SecurityTokenDescriptor tokenDescriptor = new()
		{
			Issuer = issuer,
			Audience = audience,
			Subject = claims,
			Expires = DateTime.UtcNow.AddMinutes
			(
				tipo == "rt-jwt"
					? config.MinutesToExpiresRefreshToken
					: config.MinutesToExpiresRecoveryToken
			),
			SigningCredentials = CreateSigning(config.SecretKey),
			TokenType = tipo
		};

		JwtSecurityTokenHandler tokenHandler = new();

		return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
	}
	private static SymmetricSecurityKey CreateKey(string secretKey) => new(Encoding.ASCII.GetBytes(secretKey));
	private static (string issuer, string audience) Descriptor(TokenConfigDTO config, bool refreshToken)
	{
		return
		(
			issuer: refreshToken ? config.Issuer.AbsoluteUri + "rt" : config.Issuer.AbsoluteUri,
			audience: refreshToken ? config.Audience.AbsoluteUri + "rt" : config.Audience.AbsoluteUri
		);
	}
}