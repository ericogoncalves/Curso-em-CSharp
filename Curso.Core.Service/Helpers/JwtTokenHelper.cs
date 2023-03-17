using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Curso.Core.Service.Helpers
{
    public static class JwtTokenHelper
    {
        public async static Task<JwtSecurityToken> DecodeTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            return jsonToken as JwtSecurityToken;
        }

        public static JwtSecurityToken DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            return jsonToken as JwtSecurityToken;
        }
    }
}
