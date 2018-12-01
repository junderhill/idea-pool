using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyIdeaPool.ViewModels;

namespace MyIdeaPool.Data
{
    public class JwtTokenManager : ITokenManager
    {
        private readonly IOptions<AuthenticationSettings> _authenticationSettings;
        private readonly IUserManager _userManager;

        public JwtTokenManager(IOptions<AuthenticationSettings> authenticationSettings, IUserManager userManager)
        {
            _authenticationSettings = authenticationSettings;
            _userManager = userManager;
        }

        public async Task<TokenResponse> GenerateTokenResponse(string username, bool includeRefresh = true)
        {
            var jwt = new JwtSecurityTokenHandler().WriteToken(await CreateToken(username));
          
            if(includeRefresh){
              var refresh_token = await CreateRefreshToken(username);
              return new TokenWithRefreshResponse(){
                jwt = jwt,
                refresh_token = refresh_token
              };
            }

            return new TokenResponse()
            {
              jwt = jwt
            };
        }

        private async Task<string> CreateRefreshToken(string username)
        {
            var refreshToken = Guid.NewGuid().ToString("N");
            await _userManager.UpdateRefreshTokenForUser(username, refreshToken);
            return refreshToken;
        }

        public async Task<JwtSecurityToken> CreateToken(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id), 
                new Claim(ClaimTypes.Name, username),
                new Claim("Fullname", user.Fullname),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "ideapool.junderhill.com",
                audience: "ideapool.junderhill.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(_authenticationSettings.Value.TokenExpiryMinutes),
                signingCredentials: credentials
            );
            return token;
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_authenticationSettings.Value.SymmetricSecurityKey));
        }
    }
}
