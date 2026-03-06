using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FronteraRadio.Core.Interfaces
{
    public interface IFirebaseService
    {
        void LogEvent(string eventName);
        void LogEvent(string eventName, Dictionary<string, object> parameters);
    }
}