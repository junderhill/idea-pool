using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyIdeaPool.Data;
using MyIdeaPool.Models;
using MyIdeaPool.Validators;
using MyIdeaPool.ViewModels;

namespace MyIdeaPool.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IValidator<UserSignupViewModel> _validator;
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;
        private readonly ITokenManager _tokenManager;

        public UsersController(IValidator<UserSignupViewModel> validator, IMapper mapper, IUserManager userManager, ITokenManager tokenManager)
        {
            _validator = validator;
            _mapper = mapper;
            _userManager = userManager;
            _tokenManager = tokenManager;
        }

        [Route("me")]
        [HttpGet]
        public ActionResult UserInfo()
        {
            return Ok(new
            {
                email = ControllerContext.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Email).Value,
                name = ControllerContext.HttpContext.User.Claims.Single(c => c.Type == "Fullname").Value
            });
        }
        
       [Route("users")]
       [HttpPost] 
       [AllowAnonymous]
       public async Task<ActionResult> Signup([FromBody]UserSignupViewModel model)
       {
           var validationResult = _validator.Validate(model);
           if (!validationResult.IsValid)
               return BadRequest(validationResult.ToModelStateDictionary());

           var user = _mapper.Map<UserSignupViewModel, User>(model);

           var result = await _userManager.CreateAsync(user, model.password);
           if(result.Succeeded){
               var responseToken = await _tokenManager.GenerateTokenResponse(user.UserName);
               return Ok(responseToken);
           }
           return BadRequest(result);
       }
    }
}
