using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IExodataResolver
    {
        Maybe<TExodata> Resolve<TExodata, TContext, TSubject>(IExodataRequest<TContext, TSubject> request);
    }
}
