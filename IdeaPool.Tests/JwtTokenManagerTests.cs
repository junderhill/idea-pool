using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using MyIdeaPool;
using MyIdeaPool.Data;
using MyIdeaPool.Models;
using Xunit;

namespace IdeaPool.Tests
{
    public class JwtTokenManagerTests
    {
        private readonly IOptions<AuthenticationSettings> authSettingsOption;
        private AuthenticationSettings authenticationSettings;
        private readonly IUserManager userManager;

        public JwtTokenManagerTests()
        {
            userManager = A.Fake<IUserManager>();
            A.CallTo(() => userManager.FindByNameAsync(A<string>.Ignored)).Returns(new User
            {
                Id = "abc",
                Fullname = "abc",
                Email = "email",
                UserName = "username"
            });
            authSettingsOption = A.Fake<IOptions<AuthenticationSettings>>();
            authenticationSettings = A.Fake<AuthenticationSettings>();
            A.CallTo(() => authenticationSettings.SymmetricSecurityKey).Returns("XPmOYd8JmUhN3XEXom8Vig56AnOQnkjVn7exzM9oxRkZSlX7Kf1IWabMGSNQqb1");
            A.CallTo(() => authenticationSettings.TokenExpiryMinutes).Returns(10);
            A.CallTo(() => authSettingsOption.Value).Returns(authenticationSettings);
        }
        
        [Fact]
        public void TestThatGetSymmetricSecurityKeyLoadsKeyFromConfiguration()
        {
            //arrange
            var sut = new JwtTokenManager(authSettingsOption, userManager);
            //act
            sut.GetSymmetricSecurityKey();
            //assert
            A.CallTo(() => authenticationSettings.SymmetricSecurityKey).MustHaveHappened();
        }
        
        [Fact]
        public async Task TestThatCreateTokenUsesTheExpiryTimeFromTheConfig()
        {
            //arrange
            var sut = new JwtTokenManager(authSettingsOption, userManager);
            //act
            var token = await sut.CreateToken("username");
            //assert
            A.CallTo(() => authenticationSettings.TokenExpiryMinutes).MustHaveHappened();
            //We calculate approx when we would expect the token to expire,
            //a tolerance of 5 seconds is allowed to allow for a slow running test etc.
            var approxExpectedValidToTime = DateTime.Now.AddMinutes(10);
            var timeDifference = approxExpectedValidToTime.Subtract(token.ValidTo);
            Assert.True(timeDifference.Seconds < 5); 
        }

        [Fact]
        public async Task TestThatCreationOfARefreshTokenUpdatesTheUserInTheDatabase()
        {
            //arrange
            var sut = new JwtTokenManager(authSettingsOption, userManager);
            var username = "test";
            //act
            await sut.GenerateTokenResponse(username);
            //assert
            A.CallTo(() => userManager.UpdateRefreshTokenForUser(username, A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
    }
}