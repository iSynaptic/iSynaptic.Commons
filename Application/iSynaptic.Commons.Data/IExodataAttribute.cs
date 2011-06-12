using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IExodataAttribute<TExodata> : IExodataAttribute<TExodata, TExodata>
    {
    }

    public interface IExodataAttribute<out TExodata, in THandlesExodata> where TExodata : THandlesExodata
    {
        bool ProvidesExodataFor<TRequestExodata, TContext, TSubject>(IExodataRequest<TRequestExodata, TContext, TSubject> request);
        TExodata Resolve<TContext, TSubject>(IExodataRequest<THandlesExodata, TContext, TSubject> request);
    }
}
