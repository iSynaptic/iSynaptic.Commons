using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IExodataAttribute<out TExodata>
    {
        bool ProvidesExodataFor<TRequestExodata, TContext, TSubject>(IExodataRequest<TRequestExodata, TContext, TSubject> request);
        TExodata Resolve<TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request);
    }
}
