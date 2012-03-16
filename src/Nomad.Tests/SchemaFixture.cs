using System.Linq;
using Nomad.Migrations;
using Nomad.Tests.Stubs;
using NUnit.Framework;

namespace Nomad.Tests
{
    [TestFixture]
    public class SchemaFixture
    {
        [Test]
        public void ItShouldProduceSqlForCreatingSchemaVersionTable()
        {
            var database = new DatabaseStub(0);
            var schema = new Schema(database, new LoggerStub());

            schema.CreateSchemaVersionTable();

            Assert.That(database.Statements.Any(x => x.Contains(@"CREATE TABLE [dbo].[SchemaInfo]")));
        }

        [Test]
        public void ItShouldCreateSqlForFetchingSchemaVersionWhenRequested()
        {
            var database = new DatabaseStub(1);
            var schema = new Schema(database, new LoggerStub());

            var result = schema.GetLatestSchemaVersion();

            Assert.That(database.Statements.Any(x => x.Contains(@"SELECT Max([Version])")));
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ItShouldCreateSqlForUpdatingSchemaToSpecificVersionWhenRequested()
        {
            var database = new DatabaseStub(1);
            var schema = new Schema(database, new LoggerStub());

            schema.UpdateSchemaVersionTo(10);

            Assert.That(database.Statements.Any(x => x.Contains(@"INSERT INTO [dbo].[SchemaInfo]")));
            Assert.That(database.Statements.Any(x => x.Contains(@"10")));
        }

        [Test]
        public void ItShouldCallCreateSchemaTableMethodIfEnsureSchemaVersionTableAndTableNotExists()
        {
            var database = new DatabaseStub(0);
            var schema = new Schema(database, new LoggerStub());

            schema.EnsureSchemaVersionTable();

            Assert.That(database.Statements.Any(x => x.Contains(@"CREATE TABLE [dbo].[SchemaInfo]")));
        }

        [Test]
        public void ItShouldCallNotCreateSchemaTableMethodIfEnsureSchemaVersionTableAndTableExists()
        {
            var database = new DatabaseStub(1);
            var schema = new Schema(database, new LoggerStub());

            schema.EnsureSchemaVersionTable();

            Assert.That(database.Statements.Any(x => x.Contains(@"CREATE TABLE [dbo].[SchemaInfo]")), Is.False);
        }
    }
}
