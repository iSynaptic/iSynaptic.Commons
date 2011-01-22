using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataBinding<TMetadata, TSubject>
    {
        bool Matches(MetadataRequest<TMetadata, TSubject> request);
        object GetScopeObject(MetadataRequest<TMetadata, TSubject> request);
        
        TMetadata Resolve(MetadataRequest<TMetadata, TSubject> request);

        IMetadataBindingSource Source { get;}
        Maybe<TSubject> Subject { get; }
        MemberInfo Member { get; }
    }
}
