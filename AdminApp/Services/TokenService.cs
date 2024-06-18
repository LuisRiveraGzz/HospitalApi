using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AdminApp.Services
{
    public class TokenService(IHttpContextAccessor httpContextAccessor)
    {
        public string? GetToken()
        {
            return httpContextAccessor.HttpContext != null ?
                 httpContextAccessor.HttpContext.Session.GetString("Token") : "";
        }

        public void SetToken(string token)
        {
            httpContextAccessor.HttpContext?.Session.SetString("Token", token);
        }

        public void RemoveToken()
        {
            httpContextAccessor.HttpContext?.Session.Remove("Token");
        }
        public string ObtenerRolDeUsuario()
        {
            string? token = GetToken();

            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                var rolClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (rolClaim != null)
                {
                    return rolClaim.Value;
                }
            }
            return "";
        }
    }
}