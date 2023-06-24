using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sln.Estoque.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sln.Estoque.Web.Auth
{
	public class TokenGenerator
	{
		private readonly string _secretKey;

        public TokenGenerator(){}

        public TokenGenerator(IOptions<JwtSettings> jwtSettings) 
        {
            _secretKey = jwtSettings.Value.SecretKey;
        }

		
        public string GenerateToken(string name, string role)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes("a9fa81bfe8bf9e80e746de648a063144d2878b2841a12bef6f4d84025bdd2c59");

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
				new Claim(ClaimTypes.Name, name),
				new Claim(ClaimTypes.Role, role)
			}),
				Expires = DateTime.UtcNow.AddHours(4),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
				Issuer = "JWTdoSistemaYF",
				Audience = "Aplicacao-YF"
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);

			return tokenString;
		}
	}
}
