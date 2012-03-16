using Nomad.Framework;
using Persistence.Migrations.Properties;

namespace Persistence.Migrations
{
    [Migration(2)]
    public class InstallUmbraco : Migration
    {
        public override void Execute()
        {
            Database.ExecuteSqlInTransaction(Resources.InstallUmbraco);
        }
    }
}
