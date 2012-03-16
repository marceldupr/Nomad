using System;

namespace Nomad.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void WriteToLog(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteToLog(string message, params object[] parameters)
        {
            Console.WriteLine(message, parameters);
        }
    }
}
