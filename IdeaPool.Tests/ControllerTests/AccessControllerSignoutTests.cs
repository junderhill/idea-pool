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
    public class AccessControllerSignoutTests
    {
        private IUserManager userManager;
        private ITokenManager tokenManager;
        private SignoutViewModel model;
        private AccessTokensController sut;

        public AccessControllerSignoutTests()
        {
            userManager = A.Fake<IUserManager>();
            tokenManager = A.Fake<ITokenManager>();
            model = new SignoutViewModel() {refresh_token = "abc123"};
            sut = new AccessTokensController(userManager, tokenManager);
        }

        [Fact]
        public async Task TestThatSignoutUpdatesUsersWithMatchingRefreshTokenToEmptyToken()
        {
            //arrange
            //act
            await sut.Signout(model);
            //assert
            A.CallTo(() => userManager.RemoveRefreshToken(model.refresh_token)).MustHaveHappenedOnceExactly();
        }
        
    }
}
