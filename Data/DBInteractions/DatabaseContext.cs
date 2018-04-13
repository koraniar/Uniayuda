using System.Data.Entity;
using Entities.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Cross;

namespace Data.DBInteractions
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Country> Country { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<Profession> Profession { get; set; }
        public DbSet<Purchase> Purchase { get; set; }

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
