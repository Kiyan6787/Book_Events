using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Book_Events.Plugins.Logging
{
    public sealed class Log : ILog
    {
        public Log() { }

        private static readonly Log LogInstance = new Log();

        public static Log Instance()
        {
            return LogInstance;
        }

        public void LogException(string message)
        {
            Debug.WriteLine("Log Exception: " + message);
        }

        public void LogInformation(string message)
        {
            Debug.WriteLine("Log Information: " + message);
        }
    }
}
