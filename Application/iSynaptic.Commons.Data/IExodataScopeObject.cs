using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IExodataScopeObject
    {
        bool IsInScope<TExodata, TContext, TSubject>(IExodataBinding binding, IExodataRequest<TExodata, TContext, TSubject> request);
        
        event EventHandler CacheFlushRequested;
    }
}
