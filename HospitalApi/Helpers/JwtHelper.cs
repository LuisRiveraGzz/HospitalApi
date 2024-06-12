using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalApi.Helpers
{
    public class JwtHelper
    {
        IConfiguration? Configuration { get; set; }
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string GetToken(string username, string role, int id)
        {
            JwtSecurityTokenHandler handler = new();
            if (Configuration != null)
            {
                var issuer = Configuration.GetSection("Jwt").GetValue<string>("Issuer") ?? "";
                var audience = Configuration.GetSection("Jwt").GetValue<string>("Audience") ?? "";
                var secret = Configuration.GetSection("Jwt").GetValue<string>("Secret");
                List<Claim> basicas =
                [
                    new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim("id", id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, issuer),
                new Claim (JwtRegisteredClaimNames.Aud, audience)
                ];

                JwtSecurityToken jwt = new(
                    issuer,
                    audience,
                    basicas,
                    DateTime.Now,
                    DateTime.Now.AddDays(1),
                    new SigningCredentials
                    (new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? "")),
                    SecurityAlgorithms.HmacSha256)
                    );

                return handler.WriteToken(jwt);
            }
            return "Servidor no configurado";
        }
    }
}
