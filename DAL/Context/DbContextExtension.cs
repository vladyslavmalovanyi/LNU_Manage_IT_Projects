using DAL.Moldels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.Context
{
    public static class DbContextExtension
    {

        public static bool AllMigrationsApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
            //return false;
        }

        public static void EnsureSeeded(this ApiContext context)
        {

            if (!context.Users.Any())
            {
                var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("seed" + Path.DirectorySeparatorChar + "user.json"));
                context.AddRange(users);
                context.SaveChanges();
            }

            //Ensure we have some status
            if (!context.Events.Any())
            {
                var events = JsonConvert.DeserializeObject<List<Event>>(File.ReadAllText(@"seed" + Path.DirectorySeparatorChar + "event.json"));
                context.AddRange(events);
                context.SaveChanges();
            }
        }

    }
}
