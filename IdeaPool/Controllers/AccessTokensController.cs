using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyIdeaPool.Data;
using MyIdeaPool.ViewModels;

namespace MyIdeaPool.Controllers
{
    [Route("access-tokens")]
    [ApiController]
    public class AccessTokensController : ControllerBase
    {
        private readonly ITokenManager _tokenManager;
        private readonly IUserManager _userManager;

        public AccessTokensController(IUserManager userManager, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
        }

        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult> Refresh([FromBody] RefreshAccessTokenViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.refresh_token))
                return BadRequest();
            var user = _userManager.FindByRefreshToken(model.refresh_token);
            if (user == null)
                return BadRequest();

            var token = await _tokenManager.GenerateTokenResponse(user.UserName, false);
            return Ok(token);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Signin([FromBody] SigninViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.email);
            if (user == null)
                return BadRequest();

            var validPassword = await _userManager.CheckPasswordAsync(user, model.password);
            if (!validPassword)
                return BadRequest();

            var tokenResponse = await _tokenManager.GenerateTokenResponse(model.email);

            return Ok(tokenResponse);
        }

        [HttpDelete]
        public async Task<ActionResult> Signout([FromBody] SignoutViewModel model)
        {
            await _userManager.RemoveRefreshToken(model.refresh_token);
            return Ok();
        }
    }
}