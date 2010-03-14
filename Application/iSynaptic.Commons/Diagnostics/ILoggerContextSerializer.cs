using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Diagnostics
{
    public interface ILoggerContextSerializer
    {
        string SerializeContext(object context);
    }
}
