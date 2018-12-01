using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyIdeaPool.Controllers;
using MyIdeaPool.Data;
using MyIdeaPool.Models;
using Xunit;

namespace IdeaPool.Tests.ControllerTests
{
    public class IdeaControllerTests
    {
        private IValidator<IdeaViewModel> validator;
        private IdeasController sut;
        private IdeaViewModel model;
        private IMapper mapper;
        private IIdeaPoolContext context;

        public IdeaControllerTests()
        {
            model = new IdeaViewModel()
            {
                content = "some content",
                ease = 4,
                impact = 3,
                confidence = 4
            };
            mapper = A.Fake<IMapper>();
            validator = A.Fake<IValidator<IdeaViewModel>>();
            context = A.Fake<IIdeaPoolContext>();
            sut = new IdeasController(validator, mapper, context);
            var claimsPrincipal = A.Fake<ClaimsPrincipal>();
            A.CallTo(() => claimsPrincipal.Claims).Returns(
                new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
                });
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.User = claimsPrincipal;
        }
        
        [Fact]
        public void TestThatCreateIdeaValidatesTheIdea()
        {
            //arrange
            //act
            sut.CreateIdea(model);
            //assert
            A.CallTo(() => validator.Validate(model)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void TestThatCreateIdeaWithInvalidModelRetuns400()
        {
            //arrange
            var validationResult = A.Fake<ValidationResult>();
            A.CallTo(() => validator.Validate(model)).Returns(validationResult);
            //act
            var result = sut.CreateIdea(model);
            //assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void TestThatCreateIdeaThatIsValidIsMappedToIdea()
        {
            //arrange
            SetValidatorToReturnIsValid();
            A.CallTo(() => mapper.Map<IdeaViewModel, Idea>(model)).Returns(new Idea(){});
            //act
            sut.CreateIdea(model);
            //assert
            A.CallTo(() => mapper.Map<IdeaViewModel, Idea>(model)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void TestThatCreateIdeaIsSavedToTheDatabase()
        {
            //arrange
            SetValidatorToReturnIsValid(); 
            A.CallTo(() => mapper.Map<IdeaViewModel, Idea>(model)).Returns(new Idea(){});
            //act
            sut.CreateIdea(model);
            //assert
            A.CallTo(() => context.Ideas.Add(A<Idea>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => context.SaveChanges());
        }
        
        private void SetValidatorToReturnIsValid()
        {
            var validationResult = A.Fake<ValidationResult>();
            A.CallTo(() => validationResult.IsValid).Returns(true);
            A.CallTo(() => validator.Validate(A<IdeaViewModel>.Ignored)).Returns(validationResult);
        }
    }
}