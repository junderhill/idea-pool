using FakeItEasy;
using MyIdeaPool.Controllers;
using MyIdeaPool.Data;
using MyIdeaPool.Models;
using MyIdeaPool.ViewModels;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IdeaPool.Tests.ControllerTests
{
    public class AccessControllerSigninTests
    {
        private IUserManager userManager;
        private ITokenManager tokenManager;
        private SigninViewModel model;
        private AccessTokensController sut;

        public AccessControllerSigninTests()
        {
            userManager = A.Fake<IUserManager>();
            tokenManager = A.Fake<ITokenManager>();
            model = new SigninViewModel() {email = "test", password = "test"};
            sut = new AccessTokensController(userManager, tokenManager);
        }
        
        [Fact]
        public async Task TestThatSignInChecksCredentialsAgainstUserManager()
        {
            //arrange
            //act
            await sut.Signin(model);
            //assert
            A.CallTo(() => userManager.FindByEmailAsync(model.email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.CheckPasswordAsync(A<User>.Ignored, model.password)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task TestThatIfUserIsNotFoundA400ResponseIsReturned()
        {
          //arrange
          A.CallTo(() => userManager.FindByEmailAsync(model.email)).Returns((User) null);
          //act
          var result = await sut.Signin(model); 
          //assert
          Assert.IsType<BadRequestResult>(result); 
        }

        [Fact]
        public async Task TestThatIfPasswordIsInvalidBadRequestIsReturned()
        {
          //arrange
          A.CallTo(() => userManager.FindByEmailAsync(model.email)).Returns(new User(){});
          A.CallTo(() => userManager.CheckPasswordAsync(A<User>.Ignored, model.password)).Returns(false);
          //act
          var result = await sut.Signin(model); 
          //assert
          Assert.IsType<BadRequestResult>(result); 
        }

        [Fact]
        public async Task TestThatIfValidEmailAndPasswordTokenIsGenerated()
        {
          //arrange
          A.CallTo(() => userManager.FindByEmailAsync(model.email)).Returns(new User(){});
          A.CallTo(() => userManager.CheckPasswordAsync(A<User>.Ignored, model.password)).Returns(true);
          //act
          var result = await sut.Signin(model); 
          //assert
          A.CallTo(() => tokenManager.GenerateTokenResponse(model.email, true)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task TestThatIfValidEmailAndPassword200ResponsewithTokenIsReturned()
        {
          //arrange
          A.CallTo(() => userManager.FindByEmailAsync(model.email)).Returns(new User(){});
          A.CallTo(() => userManager.CheckPasswordAsync(A<User>.Ignored, model.password)).Returns(true);
          A.CallTo(() => tokenManager.GenerateTokenResponse(model.email, true)).Returns(new TokenWithRefreshResponse(){jwt="testjwt", refresh_token="abc123"});
          //act
          var result = await sut.Signin(model); 
          //assert
          var okResult = Assert.IsType<OkObjectResult>(result);
          var tokenResponse = Assert.IsType<TokenWithRefreshResponse>(okResult.Value);
          Assert.Equal("testjwt", tokenResponse.jwt);
          Assert.Equal("abc123", tokenResponse.refresh_token);
        }

    }
}
