using Cross;
using Entities.Entities;
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
            #region Countries
            Country c1 = new Country()
            {
                Id = Guid.Parse("314ba0c9-454d-4314-9848-ad23b9ce169c"),
                Name = "Colombia"
            };

            Country c2 = new Country()
            {
                Id = Guid.Parse("329f7439-f94b-415c-a410-9044e0a5f21f"),
                Name = "Argentina"
            };

            Country c3 = new Country()
            {
                Id = Guid.Parse("1ab4f64d-f647-4445-b625-0449426dd68c"),
                Name = "EEUU"
            };

            Country c4 = new Country()
            {
                Id = Guid.Parse("a0d6b5e9-9036-4067-b314-1b01e1b845bf"),
                Name = "Rusia"
            };

            Country c5 = new Country()
            {
                Id = Guid.Parse("857eb717-23d4-4e88-873f-52e49362f3e4"),
                Name = "Brasil"
            };

            Country c6 = new Country()
            {
                Id = Guid.Parse("c90118f3-3ff2-4c4d-9242-1545f8ad5659"),
                Name = "Alemania"
            };

            Country c7 = new Country()
            {
                Id = Guid.Parse("2b8ff61b-89a3-4c9b-967b-29b0102c6a6e"),
                Name = "Canada"
            };

            Country c8 = new Country()
            {
                Id = Guid.Parse("0e0afaf7-becd-48a9-a135-3a6dde261f1b"),
                Name = "España"
            };

            Country cn = new Country()
            {
                Id = new Guid(Constants.CountryNone),
                Name = "None"
            };
            #endregion

            #region Professions

            Profession p1 = new Profession()
            {
                Id = Guid.Parse("075bee17-61c6-414d-829c-0259ee16dbc5"),
                Name = "Ingeniero"
            };

            Profession p2 = new Profession()
            {
                Id = Guid.Parse("f6157d2a-1fe9-4056-985e-3560d75e2c21"),
                Name = "Administrador"
            };

            Profession p3 = new Profession()
            {
                Id = Guid.Parse("26af3278-0ab3-44fe-a248-8014b9f6d7ce   "),
                Name = "Youtuber"
            };

            Profession pr = new Profession()
            {
                Id = new Guid(Constants.ProfessionNone),
                Name = "None"
            };
            #endregion

            #region Users
            User u1 = new User()
            {
                Id = "37bbd2ca-2eae-48ea-90a6-13cdb20e2092",
                Name = "user1",
                LastName = "user1",
                UserName = "User1",
                Email = "test@test.com",
                ProfessionId = Guid.Parse("075bee17-61c6-414d-829c-0259ee16dbc5"),
                CountryId = Guid.Parse("0e0afaf7-becd-48a9-a135-3a6dde261f1b"),
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
                ProfessionId = Guid.Parse("26af3278-0ab3-44fe-a248-8014b9f6d7ce"),
                CountryId = Guid.Parse("2b8ff61b-89a3-4c9b-967b-29b0102c6a6e"),
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
                ProfessionId = Guid.Parse("f6157d2a-1fe9-4056-985e-3560d75e2c21"),
                CountryId = Guid.Parse("c90118f3-3ff2-4c4d-9242-1545f8ad5659"),
                EmailConfirmed = true,
                PasswordHash = "AJiwJpQuDFoGgIpOW6Uoz58501GBMZYNpw8FmvyMl7+f15vfR8xT303XbpDMBR+0mw==",
                SecurityStamp = "8ea99d86-0dec-46cd-a567-dbe0ee6ac1a7",
                BornDate = DateTime.Now
            };
            #endregion

            context.Country.AddOrUpdate(c1);
            context.Country.AddOrUpdate(c2);
            context.Country.AddOrUpdate(c3);
            context.Country.AddOrUpdate(c4);
            context.Country.AddOrUpdate(c5);
            context.Country.AddOrUpdate(c6);
            context.Country.AddOrUpdate(c7);
            context.Country.AddOrUpdate(c8);
            context.Country.AddOrUpdate(cn);

            context.Profession.AddOrUpdate(p1);
            context.Profession.AddOrUpdate(p2);
            context.Profession.AddOrUpdate(p3);
            context.Profession.AddOrUpdate(pr);

            context.Users.AddOrUpdate(u1);
            context.Users.AddOrUpdate(u2);
            context.Users.AddOrUpdate(u3);

            context.SaveChanges();
        }
    }
}
