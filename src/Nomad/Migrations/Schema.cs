using Nomad.Framework;
using Nomad.Logging;
using Nomad.Properties;

namespace Nomad.Migrations
{
    public class Schema : ISchema
    {
        private const int SCHEMA_MINIMUM = -1;
        private readonly IDatabase database;
        private readonly ILogger logger;

        public Schema(IDatabase database, ILogger logger)
        {
            this.database = database;
            this.logger = logger;
        }

        public IDatabase Database
        {
            get { return database; }
        }

        public int GetLatestSchemaVersion()
        {
            var result = Database.ExecuteScalar(Resources.GetLatestVersion);
            return ResultOrMinimum(result);
        }

        private static int ResultOrMinimum(object result)
        {
            if (ResultIsNotEmpty(result))
                return int.Parse(result.ToString());

            return SCHEMA_MINIMUM;
        }

        private static bool ResultIsNotEmpty(object result)
        {
            return !string.IsNullOrEmpty(string.Concat(string.Empty, result));
        }

        public void UpdateSchemaVersionTo(long version)
        {
            var sqlToExecute = string.Format(Resources.InsertSchemaVersion, version);
            Database.ExecuteNonQuery(sqlToExecute);
        }

        public void EnsureSchemaVersionTable()
        {
            if (SchemaTableCountIsZero)
            {
               CreateSchemaVersionTable();
            }
        }

        public bool SchemaTableCountIsZero
        {
            get { return (int) Database.ExecuteScalar(Resources.GetSchemaVersionTableCount) == 0; }
        }

        public void CreateSchemaVersionTable()
        {
            logger.WriteToLog(@"Schema Table Doesn't Exist... Creating...");
            Database.ExecuteNonQuery(Resources.CreateSchemaInfoTable);
        }
    }
}
