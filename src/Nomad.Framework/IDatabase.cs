namespace Nomad.Framework
{
    public interface IDatabase
    {
        void ExecuteNonQuery(string statement);
        void ExecuteSqlInTransaction(string scriptContents);
        object ExecuteScalar(string script);
    }
}
