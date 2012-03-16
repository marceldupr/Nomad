using System;
using Nomad.Framework;
using Persistence.Migrations.Properties;

namespace Persistence.Migrations
{
    [Migration(1)]
    public class CreateDefaultUser : Migration
    {
        public override void Execute()
        {
            var script = string.Format(Resources.CreateDefaultUser, Settings.Database, Settings.User, Settings.Password);
            Console.WriteLine(script);
            Database.ExecuteSqlInTransaction(script);
        }
    }
}
