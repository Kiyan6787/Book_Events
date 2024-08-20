using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Plugins.Logging
{
    public interface ILog
    {
        void LogException(string message);
        void LogInformation(string message);
    }
}
