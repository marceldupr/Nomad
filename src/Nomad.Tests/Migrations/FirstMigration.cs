using Nomad.Framework;

namespace Nomad.Tests.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        public override void Execute()
        {
            Database.ExecuteSqlInTransaction("FirstMigration");
        }
    }
}
