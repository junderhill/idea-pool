using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyIdeaPool.Models;

namespace MyIdeaPool.Data
{
    public class IdeaPoolContext : IdentityDbContext<User>, IIdeaPoolContext
    {
        public IdeaPoolContext(DbContextOptions option):base(option)
        {
        }

        public DbSet<Idea> Ideas { get; set; }
    }
}