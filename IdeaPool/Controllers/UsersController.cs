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
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IValidator<UserSignupViewModel> _validator;
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;

        public UsersController(IValidator<UserSignupViewModel> validator, IMapper mapper, IUserManager userManager)
        {
            _validator = validator;
            _mapper = mapper;
            _userManager = userManager;
        }

       [HttpPost] 
       public async Task<ActionResult> Signup([FromBody]UserSignupViewModel model)
       {
           var validationResult = _validator.Validate(model);
           if (!validationResult.IsValid)
               return BadRequest(validationResult.ToModelStateDictionary());

           var user = _mapper.Map<UserSignupViewModel, User>(model);

           var result = await _userManager.CreateAsync(user, model.password);
          
           return Ok();
       }
    }
}