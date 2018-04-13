using Data.DBInteractions;
using Logic.Interfaces;
using System.Data.Entity;

namespace Logic.Services
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        public void Initialize()
        {

            Database.SetInitializer<DatabaseContext>(new MigrateDatabaseToLatestVersion<DatabaseContext, Data.Migrations.Configuration>());

            using (DatabaseContext db = new DatabaseContext())
            {
                db.Database.Initialize(true);
            }
        }
    }
}
