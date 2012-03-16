using System;
using Nomad.Migrations;

namespace Nomad
{
    public class MigrationException : Exception
    {
        public MigrationException(ChangeSet changeSet, string message) : base(string.Concat(message, "\nMigration Version: ", changeSet.Version)) {}
    }
}
