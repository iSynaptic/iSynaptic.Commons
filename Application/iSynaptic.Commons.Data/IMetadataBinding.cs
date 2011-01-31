using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataBinding<out TMetadata, in TSubject>
    {
        bool Matches(IMetadataRequest<TMetadata, TSubject> request);
        object GetScopeObject(IMetadataRequest<TMetadata, TSubject> request);

        TMetadata Resolve(IMetadataRequest<TMetadata, TSubject> request);

        IMetadataBindingSource Source { get; }

        bool BoundToSubjectInstance { get; }

        Type SubjectType { get; }
    }
}
