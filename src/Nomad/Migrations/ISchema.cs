using Nomad.Framework;

namespace Nomad.Migrations
{
    public interface ISchema
    {
        IDatabase Database { get; }
        bool SchemaTableCountIsZero { get; }
        int GetLatestSchemaVersion();
        void UpdateSchemaVersionTo(int version);
        void EnsureSchemaVersionTable();
        void CreateSchemaVersionTable();
    }
}