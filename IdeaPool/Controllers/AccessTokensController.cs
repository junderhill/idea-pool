using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MyIdeaPool.Data;
using MyIdeaPool.Models;
using MyIdeaPool.Validators;
using MyIdeaPool.ViewModels;

namespace MyIdeaPool.Controllers
{
    [Route("access-tokens")]
    [ApiController]
    public class AccessTokensController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly ITokenManager _tokenManager;

        public AccessTokensController(IUserManager userManager, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
        }

       [HttpPost] 
       [Route("refresh")]
       public async Task<ActionResult> Refresh([FromBody]RefreshAccessTokenViewModel model)
       {
          var user = _userManager.FindByRefreshToken(model.refresh_token);
          if(user == null)
            return BadRequest();

          var token = await _tokenManager.GenerateTokenResponse(user.UserName, false);
          return Ok(token);
       }
    }
}
