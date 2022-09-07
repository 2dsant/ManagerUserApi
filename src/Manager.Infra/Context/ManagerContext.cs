using Microsoft.EntityFrameworkCore;
using Manager.Domain.Entities;
using Manager.Infra.Mappings;

namespace Manager.Infra.Context
{
    public class ManagerContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ManagerContext() { }

        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options)
        { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMap());
        }
    }
}