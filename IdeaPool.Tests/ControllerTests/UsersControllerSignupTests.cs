using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyIdeaPool.Controllers;
using MyIdeaPool.Data;
using MyIdeaPool.Models;
using MyIdeaPool.Validators;
using MyIdeaPool.ViewModels;
using Xunit;

namespace IdeaPool.Tests.ControllerTests
{
    public class UsersControllerSignupTests
    {
        private readonly IValidator<UserSignupViewModel> validator;
        private readonly IMapper mapper;
        private readonly UsersController sut;
        private readonly UserSignupViewModel model;
        private readonly IUserManager userManager;
        private readonly ITokenManager tokenManager;

        public UsersControllerSignupTests()
        {
            validator = A.Fake<IValidator<UserSignupViewModel>>();
            mapper = A.Fake<IMapper>();
            userManager = A.Fake<IUserManager>();
            sut = new UsersController(validator,mapper, userManager,tokenManager);
            model = new UserSignupViewModel
            {
                email = "test@test.com",
                name = "Jason Underhill",
                password = "Test1234"
            };
        }
        
        [Fact]
        public async Task TestThatOnSignupModelIsValidated()
        {
            //arrange
            //act
            await sut.Signup(model);
            //assert
            A.CallTo(() => validator.Validate(model)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task TestThatIfSignupModelValidationFailsA400ResponseIsReturned()
        {
            //arrange
            var validationResult = A.Fake<ValidationResult>();
            A.CallTo(() => validator.Validate(A<UserSignupViewModel>.Ignored)).Returns(validationResult);
            //act
            var result = await sut.Signup(model);
            //assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task TestThatBadRequestResponseIncludesValidationErrors()
        {
            //arrange
            A.CallTo(() => validator.Validate(A<UserSignupViewModel>.Ignored))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("password", "Password must be at least 8 characters in length.")
                }));
            //act
            var result = await sut.Signup(model);
            //assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var serialisedError = Assert.IsType<SerializableError>(badRequestObjectResult.Value);
            var passwordErrors = (string[])serialisedError["password"];
            Assert.NotEmpty(passwordErrors);
            Assert.Contains("Password must be at least 8 characters in length.", passwordErrors);
        }

        [Fact]
        public async Task TestThatAValidModelIsMappedToAUser()
        {
            //arrange
            SetValidatorToReturnIsValid();
            //act
            await sut.Signup(model);
            //assert
            A.CallTo(() => mapper.Map<UserSignupViewModel, User>(model)).MustHaveHappenedOnceExactly();
        }

        private void SetValidatorToReturnIsValid()
        {
            var validationResult = A.Fake<ValidationResult>();
            A.CallTo(() => validationResult.IsValid).Returns(true);
            A.CallTo(() => validator.Validate(A<UserSignupViewModel>.Ignored)).Returns(validationResult);
        }

        [Fact]
        public async Task TestThatWithAValidModelUserManagerCreateIsCalled()
        {
            //arrange
            SetValidatorToReturnIsValid();
            A.CallTo(() => mapper.Map<UserSignupViewModel, User>(model))
                .Returns(new User()
                {
                    Email = model.email,
                    UserName = model.email,
                    Fullname = model.name
                });
            //act
            await sut.Signup(model);
            //assert
            A.CallTo(() => userManager.CreateAsync(A<User>.Ignored, model.password)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task TestThatIfCreateOnUserManagerSucceedsOk200ResponseReturned()
        {
            //arrange
            SetValidatorToReturnIsValid();
            A.CallTo(() => tokenManager.GenerateTokenResponse(A<string>.Ignored)).Returns(new TokenResponse(){jwt = "abc123",refresh_token = "abc123"});
            A.CallTo(() => mapper.Map<UserSignupViewModel, User>(model))
                .Returns(new User()
                {
                    Email = model.email,
                    UserName = model.email,
                    Fullname = model.name
                });
            A.CallTo(() => userManager.CreateAsync(A<User>.Ignored, model.password)).Returns(IdentityResult.Success);
            //act
            var result = await sut.Signup(model);
            //assert
            Assert.IsType<OkResult>(result);
        }
        
        [Fact]
        public async Task TestThatIfCreateOnUserManagerFails400ResponseIsReturned()
        {
            //arrange
            SetValidatorToReturnIsValid();
            A.CallTo(() => mapper.Map<UserSignupViewModel, User>(model))
                .Returns(new User
                {
                    Email = model.email,
                    UserName = model.email,
                    Fullname = model.name
                });
            A.CallTo(() => userManager.CreateAsync(A<User>.Ignored, model.password)).Returns(IdentityResult.Failed(new IdentityError(){Code = "DuplicateEmail", Description = $"Email '{model.email}' is already taken."}));
            //act
            var result = await sut.Signup(model);
            //assert
            var badrequest = Assert.IsType<BadRequestObjectResult>(result);
            var identityResult = Assert.IsType<IdentityResult>(badrequest.Value);
            Assert.NotNull(identityResult);
            Assert.True(identityResult.Errors.Any(e => e.Code == "DuplicateEmail"));
        }
    }
}