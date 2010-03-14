using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Diagnostics
{
    public interface ILogger
    {
        void Log<T>(LogLevel level, object message, T context);
    }
}
