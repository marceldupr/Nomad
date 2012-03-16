using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nomad.Framework;
using Nomad.Logging;
using Nomad.Properties;

namespace Nomad.Data
{
    public class Database : IDatabase
    {
        private readonly string connectionString;
        private readonly ILogger logger;
        private const string CONNECTION_STRING_FORMAT_SQLUSER = "Data Source={0}; Initial Catalog={1};User Id={2};Password={3}";
        private const string MASTER_CONNECTION_STRING_FORMAT_SQLUSER = "Data Source={0}; Initial Catalog=Master;User Id={1};Password={2}";

        private const string CONNECTION_STRING_FORMAT_INT_SEC = "Data Source={0}; Initial Catalog={1}; Integrated Security=SSPI";
        private const string MASTER_CONNECTION_STRING_FORMAT_INT_SEC = "Data Source={0}; Initial Catalog=Master; Integrated Security=SSPI";


        public Database(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            this.logger = logger;
        }
        
        public Database(string server, string database, string user, string password, ILogger logger)
        {
            connectionString = EnsureDatabase(server, database, user, password);
            this.logger = logger;
        }

        public Database(string server, string database, ILogger logger)
        {
            connectionString = EnsureDatabase(server, database);
            this.logger = logger;
        }

        public string ConnectionString
        {
            get { return connectionString; }
        }

        private static string EnsureDatabase(string server, string database)
        {
            string script = string.Format(Resources.CreateDatabase, database);
            using (var connection = new SqlConnection(string.Format(MASTER_CONNECTION_STRING_FORMAT_INT_SEC, server)))
            {
                connection.Open();
                var command = new SqlCommand(script, connection);
                command.ExecuteNonQuery();
            }

            return string.Format(CONNECTION_STRING_FORMAT_INT_SEC, server, database);
        }

        private static string EnsureDatabase(string server, string database, string user, string password)
        {
            string script = string.Format(Resources.CreateDatabase, database);
            using (var connection = new SqlConnection(string.Format(MASTER_CONNECTION_STRING_FORMAT_SQLUSER, server, user, password)))
            {
                connection.Open();
                var command = new SqlCommand(script, connection);
                command.ExecuteNonQuery();
            }

            return string.Format(CONNECTION_STRING_FORMAT_SQLUSER, server, database, user, password);
        }

        public void ExecuteNonQuery(string statement)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(statement, connection);
                command.ExecuteNonQuery();
            }
        }

        public void ExecuteSqlInTransaction(string scriptContents)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    foreach (string individualStatement in SplitSqlOnGo(scriptContents))
                    {
                        var command = new SqlCommand(individualStatement, connection) {Transaction = transaction};
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.WriteToLog(ex.ToString());
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public object ExecuteScalar(string script)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(script, connection);
                return command.ExecuteScalar();
            }
        }

        private static IEnumerable<string> SplitSqlOnGo(string sql)
        {
            var splitter = new[] { "GO" };
            return sql.Split(splitter,
                             StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
