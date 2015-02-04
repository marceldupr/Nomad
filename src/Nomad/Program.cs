using System;
using System.Reflection;
using Nomad.Data;
using Nomad.Framework;
using Nomad.Logging;
using Nomad.Migrations;

namespace Nomad
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var logger = new ConsoleLogger();
            try
            {
                IDatabase database = CreateDatabaseFromArgs(args, logger);
                if (database != null)
                {
                    var migrator = new Migrator(new Schema(database, logger), Assembly.LoadFrom(args[0]), logger);
                    migrator.MigrateToLatest();
                } else
                {
                    throw new ArgumentException("Invalid Arguments");
                }
            }
            catch (Exception ex)
            {
                logger.WriteToLog("An error occurred while trying to migrate. \nUsage: Nomad ':assemblyfile' [:connectionstring]\nNomad ':assemblyfile' ':server' ':db' [':user' ':password']");
                logger.WriteToLog("Error Details: {0}", ex);
                return -1;
            }

            return 0;
        }

        private static IDatabase CreateDatabaseFromArgs(string[] args, ConsoleLogger logger)
        {
            IDatabase database = null;
            if (args.Length == 5)
            {
                database = CreateSqlUserDatabase(args, logger);
            } else if(args.Length == 3)
            {
                database = CreateIntegratedSecurityDatabase(args, logger);
            } else if(args.Length == 2)
            {
                database = UseExistingDatabase(args, logger);
            }
            return database;
        }

        private static IDatabase UseExistingDatabase(string[] args, ConsoleLogger logger)
        {
            return new Database(args[1], logger);
        }

        private static IDatabase CreateIntegratedSecurityDatabase(string[] args, ConsoleLogger logger)
        {
            return new Database(args[1], args[2], logger);
        }

        private static IDatabase CreateSqlUserDatabase(string[] args, ConsoleLogger logger)
        {
            return new Database(args[1], args[2], args[3], args[4], logger);
        }
    }
}
