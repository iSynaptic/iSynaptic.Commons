using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataBinding
    {
        bool Matches<TMetadata, TSubject>(IMetadataRequest<TMetadata, TSubject> request);
        object GetScopeObject<TMetadata, TSubject>(IMetadataRequest<TMetadata, TSubject> request);

        TMetadata Resolve<TMetadata, TSubject>(IMetadataRequest<TMetadata, TSubject> request);

        IMetadataBindingSource Source { get; }

        bool BoundToSubjectInstance { get; }

        Type SubjectType { get; }
    }
}
