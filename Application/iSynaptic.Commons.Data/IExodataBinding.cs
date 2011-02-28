using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IExodataBinding
    {
        bool Matches<TExodata, TSubject>(IExodataRequest<TSubject> request);
        object GetScopeObject<TExodata, TSubject>(IExodataRequest<TSubject> request);

        TExodata Resolve<TExodata, TSubject>(IExodataRequest<TSubject> request);

        IExodataBindingSource Source { get; }

        bool BoundToSubjectInstance { get; }

        Type SubjectType { get; }
    }
}
