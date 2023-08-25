using Auth.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<LeadEntity> Lead { get; set; }

        public DbSet<ForumMessages> ForumMessages { get; set; }

        public DbSet<ForumTopic> ForumTopic { get; set; }

        public DbSet<ApplicationUser> AplicationUser { get; set; }

    }
}
