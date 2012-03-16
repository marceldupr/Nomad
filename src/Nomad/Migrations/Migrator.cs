using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nomad.Logging;

namespace Nomad.Migrations
{
    public class Migrator
    {
        private readonly ISchema schema;
        private readonly Assembly assembly;
        private readonly ILogger logger;

        public Migrator(ISchema schema, Assembly assembly, ILogger logger)
        {
            this.schema = schema;
            this.assembly = assembly;
            this.logger = logger;
        }

        public void MigrateToLatest()
        {
            logger.WriteToLog(@"Ensuring SchemaTable");

            schema.EnsureSchemaVersionTable();
            var previousSchemaApplied = schema.GetLatestSchemaVersion();
            var migrationsToApply = FindChangesSinceLastMigrationInAssembly(previousSchemaApplied, assembly);
            if (migrationsToApply.Count() > 0)
            {
                logger.WriteToLog(@"Found {0} Migrations...", migrationsToApply.Count());
                ApplyUpMigrations(migrationsToApply);
            } else
            {
                logger.WriteToLog(@"No Migrations Found");
            }
        }

        private void ApplyUpMigrations(IEnumerable<ChangeSet> migrationsToApply)
        {
            foreach (var migrationToApply in migrationsToApply)
            {
                logger.WriteToLog(@"Applying ChangeSet {0}...", migrationToApply.Version);
                migrationToApply.Change.Database = schema.Database;
                try
                {
                    migrationToApply.Change.Execute();
                    schema.UpdateSchemaVersionTo(migrationToApply.Version);
                    logger.WriteToLog(@"Setting last migration version applied to {0}", migrationToApply.Version);
                }
                catch (Exception ex)
                {
                    throw new MigrationException(migrationToApply, ex.ToString());
                }
            }
        }

        private static IEnumerable<ChangeSet> FindChangesSinceLastMigrationInAssembly(int previousSchemaApplied, Assembly assembly)
        {
            return assembly.GetChangeSetsSince(previousSchemaApplied);
        }
    }
}
