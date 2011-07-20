using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IExodataAttribute<TExodata>
    {
        Maybe<TExodata> TryResolve<TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request);
    }
}
