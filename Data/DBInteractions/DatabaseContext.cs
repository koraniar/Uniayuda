using Cross;
using Entities.DatabaseEntities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Data.DBInteractions
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Post> Post { get; set; }
        public DbSet<Assessment> Assessment { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<History> History { get; set; }

        public DatabaseContext() : base(Constants.DefaultConnectionString)
        {
        }

        public virtual bool Commit()
        {
            return base.SaveChanges() > 0;
        }

        public virtual async Task<bool> CommitAsync()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
