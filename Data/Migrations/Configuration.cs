using Entities.DatabaseEntities;
using System;
using System.Data.Entity.Migrations;

namespace Data.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<DBInteractions.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DBInteractions.DatabaseContext context)
        {
            #region Users
            User u1 = new User()
            {
                Id = "37bbd2ca-2eae-48ea-90a6-13cdb20e2092",
                Name = "user1",
                LastName = "user1",
                UserName = "User1",
                Email = "test@test.com",
                EmailConfirmed = true,
                PasswordHash = "AEIQhRJXlwEqf3fjZo+RkPhenZHlb4QZqzDxyyh/nbMTpuxWlUjivjJl/2vfqa5SPw==",
                SecurityStamp = "160d18dd-616b-4bb1-9a93-9e5d57c4c61f",
                BornDate = DateTime.Now
            };

            User u2 = new User()
            {
                Id = "79262b81-4547-4d44-8129-f4deacc130b3",
                Name = "user2",
                LastName = "user2",
                UserName = "User2",
                Email = "test2@test2.com",
                EmailConfirmed = true,
                PasswordHash = "AK1cIRi1W2JTNbqY+Rz6BdE5kMTkBz50ckhMQajDQY8u529ROwk27LSeXGqNqUqavg==",
                SecurityStamp = "91016cbc-90ef-489d-8633-b1e46cac3b92",
                BornDate = DateTime.Now
            };

            User u3 = new User()
            {
                Id = "c7c0c35e-2bc6-4c51-b178-86a062ba4b01",
                Name = "user3",
                LastName = "user3",
                UserName = "User3",
                Email = "test3@test3.com",
                EmailConfirmed = true,
                PasswordHash = "AJiwJpQuDFoGgIpOW6Uoz58501GBMZYNpw8FmvyMl7+f15vfR8xT303XbpDMBR+0mw==",
                SecurityStamp = "8ea99d86-0dec-46cd-a567-dbe0ee6ac1a7",
                BornDate = DateTime.Now
            };
            #endregion
            
            context.Users.AddOrUpdate(u1);
            context.Users.AddOrUpdate(u2);
            context.Users.AddOrUpdate(u3);

            context.SaveChanges();
        }
    }
}
