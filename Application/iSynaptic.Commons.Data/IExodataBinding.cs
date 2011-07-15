using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IExodataBinding
    {
        Maybe<TExodata> TryResolve<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request);
    }
}
