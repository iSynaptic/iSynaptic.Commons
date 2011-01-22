using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataBinding<out TMetadata, in TSubject>
    {
        bool Matches(IMetadataRequest<TSubject> request);
        object GetScopeObject(IMetadataRequest<TSubject> request);

        TMetadata Resolve(IMetadataRequest<TSubject> request);

        IMetadataBindingSource Source { get; }

        bool BoundToSubjectInstance { get; }
        bool BoundToMember { get; }

        Type SubjectType { get; }
    }
}
