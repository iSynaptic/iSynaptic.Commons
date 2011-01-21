using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataResolver
    {
        TMetadata Resolve<TMetadata, TSubject>(MetadataDeclaration<TMetadata> declaration, Maybe<TSubject> subject, MemberInfo memberInfo);
    }
}
