using System.Collections.Generic;
using Nomad.Framework;

namespace Nomad.Tests.Stubs
{
    public class DatabaseStub : IDatabase
    {
        private readonly int scalarToReturn;
        public IList<string> Statements = new List<string>();

        public DatabaseStub(int scalarToReturn)
        {
            this.scalarToReturn = scalarToReturn;
        }

        public DatabaseStub()
        {
            scalarToReturn = 0;
        }

        public void ExecuteNonQuery(string statement)
        {
            Statements.Add(statement);
        }

        public void ExecuteSqlInTransaction(string scriptContents)
        {
            Statements.Add(scriptContents);
        }

        public object ExecuteScalar(string script)
        {
            Statements.Add(script);
            return scalarToReturn;
        }
    }
}
