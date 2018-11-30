using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using MyIdeaPool.ViewModels;

namespace MyIdeaPool.Data
{
    public interface ITokenManager
    {
        JwtSecurityToken CreateToken(string username);
        Task<TokenResponse> GenerateTokenResponse(string username);
    }
}