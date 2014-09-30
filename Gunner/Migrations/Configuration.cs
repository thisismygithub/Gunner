using Gunner.Models;

namespace Gunner.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Gunner.Models.ArsenalDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Gunner.Models.ArsenalDbContext context)
        {
            int i = 0;
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Fixtures.AddOrUpdate(

              new Fixtures { Date = new DateTime(2006, 8, 19), Ground = "London", Opponent = "Aston Villa", Result = "1-1" },
              new Fixtures { Date = new DateTime(2006, 8, 26), Ground = "Manchester", Opponent = "Manchester City", Result = "1-0" },
              new Fixtures { Date = new DateTime(2006, 9, 9), Ground = "London", Opponent = "Middlesbrough", Result = "1-1" }
            );

        }
    }
}
