using Microsoft.AspNetCore.Mvc;
using MyIdeaPool.ViewModels;

namespace MyIdeaPool.Controllers
{
    [Route("")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController()
        {
            
        }

       [HttpPost] 
       public void Signup([FromBody]UserSignupViewModel model)
       {

       }
    }
}