using System.Linq;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MyIdeaPool.Data;
using MyIdeaPool.Models;
using MyIdeaPool.Validators;
using MyIdeaPool.ViewModels;

namespace MyIdeaPool.Controllers
{
    [ApiController]
    [Route("ideas")]
    public class IdeasController : ControllerBase
    {
        private readonly IValidator<IdeaViewModel> _validator;
        private readonly IMapper _mapper;
        private readonly IIdeaPoolContext _context;

        public IdeasController(IValidator<IdeaViewModel> validator, IMapper mapper, IIdeaPoolContext context)
        {
            _validator = validator;
            _mapper = mapper;
            _context = context;
        }

        [HttpPost]
        public ActionResult CreateIdea([FromBody] IdeaViewModel model)
        {
            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToModelStateDictionary());

            var idea = _mapper.Map<IdeaViewModel, Idea>(model);
            SetUserID(idea);

            _context.Ideas.Add(idea);
            _context.SaveChanges();

            var response = _mapper.Map<Idea, IdeaResponse>(idea);
            return Ok(response);
        }

        private void SetUserID(Idea idea)
        {
            idea.UserId = User.Claims
                .Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}