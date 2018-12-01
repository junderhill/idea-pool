using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyIdeaPool.Models;

namespace MyIdeaPool.Data
{
    public class UserManager : UserManager<User>, IUserManager
    {
        public UserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task UpdateRefreshTokenForUser(string username, string refreshtoken)
        {
            var user = await FindByNameAsync(username);
            user.RefreshToken = refreshtoken;
            await Store.UpdateAsync(user, CancellationToken.None);
        }

        public User FindByRefreshToken(string modelRefreshToken)
        {
            var user = this.Users.FirstOrDefault(x => x.RefreshToken == modelRefreshToken);
            return user;
        }
    }
}
