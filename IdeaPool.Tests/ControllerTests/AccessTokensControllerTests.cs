using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyIdeaPool.Controllers;
using MyIdeaPool.Data;
using MyIdeaPool.ViewModels;
using MyIdeaPool.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace IdeaPool.Tests.ControllerTests
{
    public class AccessTokensControllerTests
    {
        private IUserManager userManager;
        private ITokenManager tokenManager;
        private RefreshAccessTokenViewModel model;

        public AccessTokensControllerTests()
        {
            userManager = A.Fake<IUserManager>();
            tokenManager = A.Fake<ITokenManager>();
            model = new RefreshAccessTokenViewModel()
            {
                refresh_token = "abc123ef"
            };
        }
        
        [Fact]
        public async Task TestThatRefreshTokenCallsUserManagerToGetUser()
        {
            //arrange
            var sut = new AccessTokensController(userManager, tokenManager); 
            //act
            await sut.Refresh(model);
            //assert
            A.CallTo(() => userManager.FindByRefreshToken(model.refresh_token)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task TestThatIfUserNotFoundA400ErrorIsReturned()
        {
          //arrange
          A.CallTo(() => userManager.FindByRefreshToken(A<string>.Ignored)).Returns(null);
          var sut = new AccessTokensController(userManager,tokenManager); 
          //act
          var result = await sut.Refresh(model);
          //assert
          Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task TestThatIfUserIsFoundTokenIsGenerated()
        {
          //arrange
          var user = new User(){
            UserName = "TestUser"
          };
          A.CallTo(() => userManager.FindByRefreshToken(A<string>.Ignored)).Returns(user);
          var sut = new AccessTokensController(userManager,tokenManager); 
          //act
          await sut.Refresh(model);
          //assert
          A.CallTo(() => tokenManager.GenerateTokenResponse(user.UserName, false)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task TestThatGeneratedTokenIsReturnedIn200Response(){
          //arrange
          var user = new User(){
            UserName = "TestUser"
          };
          A.CallTo(() => userManager.FindByRefreshToken(A<string>.Ignored)).Returns(user);
          A.CallTo(() => tokenManager.GenerateTokenResponse(user.UserName,false)).Returns(
              new TokenResponse(){
                jwt = "abc"
              });
          var sut = new AccessTokensController(userManager,tokenManager); 
          //act
          var result = await sut.Refresh(model);
          //assert

          Assert.IsType<OkObjectResult>(result);
        }

    }
}
