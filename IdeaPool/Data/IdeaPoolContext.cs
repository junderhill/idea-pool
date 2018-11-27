using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyIdeaPool.Models;

namespace MyIdeaPool.Data
{
    public class IdeaPoolContext : IdentityDbContext<User>
    {
        public IdeaPoolContext(DbContextOptions option):base(option)
        {
        }
    }
}