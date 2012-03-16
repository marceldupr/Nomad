namespace Nomad.Logging
{
    public interface ILogger
    {
        void WriteToLog(string message);
        void WriteToLog(string message, params object[] parameters);
    }
}
