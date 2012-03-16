using System.Linq;
using Nomad.Migrations;
using Nomad.Tests.Migrations;
using Nomad.Tests.Stubs;
using NUnit.Framework;

namespace Nomad.Tests
{
    [TestFixture]
    public class MigratorFixture
    {
        [Test]
        public void ItShouldInformIfNoMigrationsFoundInAssembly()
        {
            var database = new DatabaseStub();
            var logger = new LoggerStub();
            var schema = new Schema(database, logger);
            var migrator = new Migrator(schema, typeof (Schema).Assembly, logger);

            migrator.MigrateToLatest();

            Assert.That(logger.Logs.Any(x=>x.Contains(@"No Migrations Found")));
        }

        [Test]
        public void ItShouldFindFirstMigrationInAssembly()
        {
            var database = new DatabaseStub();
            var logger = new LoggerStub();
            var schema = new Schema(database, logger);
            var migrator = new Migrator(schema, typeof(FirstMigration).Assembly, logger);

            migrator.MigrateToLatest();

            Assert.That(logger.Logs.Any(x => x.Contains(@"Found 1 Migrations")));
        }

        [Test]
        public void ItShouldSkipFindFirstMigrationIfVersionHigher()
        {
            var database = new DatabaseStub(3);
            var logger = new LoggerStub();
            var schema = new Schema(database, logger);
            var migrator = new Migrator(schema, typeof(FirstMigration).Assembly, logger);

            migrator.MigrateToLatest();

            Assert.That(logger.Logs.Any(x => x.Contains(@"No Migrations Found")));
        }

        [Test]
        public void ItShouldApplyMigrationsSinceLastVersionAndUpdateSchemaTable()
        {
            var database = new DatabaseStub();
            var logger = new LoggerStub();
            var schema = new Schema(database, logger);
            var migrator = new Migrator(schema, typeof(FirstMigration).Assembly, logger);

            migrator.MigrateToLatest();

            Assert.That(database.Statements.Any(x => x.Contains("FirstMigration")));
        }

        [Test]
        public void ItShouldNotApplyMigrationsSinceLastVersionWhenVersionBiggerThanMigration()
        {
            var database = new DatabaseStub(3);
            var logger = new LoggerStub();
            var schema = new Schema(database, logger);
            var migrator = new Migrator(schema, typeof(FirstMigration).Assembly, logger);

            migrator.MigrateToLatest();

            Assert.That(database.Statements.Any(x => x.Contains("FirstMigration")), Is.False);
        }

        [Test]
        public void ItShouldUpdateSchemaVersionTo1IfMigrationApplicable()
        {
            var database = new DatabaseStub();
            var logger = new LoggerStub();
            var schema = new Schema(database, logger);
            var migrator = new Migrator(schema, typeof(FirstMigration).Assembly, logger);

            migrator.MigrateToLatest();

            Assert.That(database.Statements.Any(x => x.Contains(@"INSERT INTO [dbo].[SchemaInfo]
           ([Version])
     VALUES
           (1)")));
        }

        [Test]
        public void ItShouldNotUpdateSchemaVersionTo1IfMigrationNotApplicable()
        {
            var database = new DatabaseStub(3);
            var logger = new LoggerStub();
            var schema = new Schema(database, logger);
            var migrator = new Migrator(schema, typeof(FirstMigration).Assembly, logger);

            migrator.MigrateToLatest();

            Assert.That(database.Statements.Any(x => x.Contains(@"INSERT INTO [dbo].[SchemaInfo]
           ([Version])
     VALUES
           (1)")), Is.False);
        }
    }
}
