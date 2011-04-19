using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IExodataScopeObject
    {
        bool IsInScope<TExodata, TContext, TSubject>(IExodataBinding binding, IExodataRequest<TContext, TSubject> request);
        
        event EventHandler CacheFlushRequested;
    }
}
