using System;
using System.Collections.Generic;
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
        private const int PAGE_SIZE = 10;

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

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteIdea([FromRoute]Guid id)
        {
            var idea = _context.Ideas.Find(id);
            if (idea == null)
                return NotFound();
            if (idea.UserId != GetUserID())
                return NotFound();

            _context.Ideas.Remove(idea);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet]
        public ActionResult GetIdeas([FromQuery] int page =1)
        {
            var ideas = _context.Ideas.Where(x => x.UserId == GetUserID()).ToList();

            var results = ideas.OrderByDescending(i => i.Average)
                .Skip((PAGE_SIZE * (page - 1))).Take(PAGE_SIZE);

            var mappedResults = _mapper.Map<IEnumerable<Idea>, List<IdeaResponse>>(results);
            
            return Ok(mappedResults);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateIdea([FromRoute] Guid id, [FromBody] IdeaViewModel model)
        {
            var idea = _context.Ideas.SingleOrDefault(x => x.Id == id);
            if (idea == null)
                return NotFound();

            if (idea.UserId != GetUserID())
                return NotFound();

            idea = _mapper.Map(model, idea);
            _context.SaveChanges();
            
            var response = _mapper.Map<Idea, IdeaResponse>(idea);
            return Ok(response);
        }

        private void SetUserID(Idea idea)
        {
            idea.UserId = GetUserID();
        }

        private string GetUserID()
        {
            return User.Claims
                .Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}