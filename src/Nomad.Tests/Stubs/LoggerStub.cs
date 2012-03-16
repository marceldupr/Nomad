using System.Collections.Generic;
using Nomad.Logging;

namespace Nomad.Tests.Stubs
{
    public class LoggerStub : ILogger
    {
        public IList<string> Logs = new List<string>();
        
        public void WriteToLog(string message)
        {
            Logs.Add(message);
        }

        public void WriteToLog(string message, params object[] parameters)
        {
            Logs.Add(string.Format(message, parameters));
        }
    }
}
