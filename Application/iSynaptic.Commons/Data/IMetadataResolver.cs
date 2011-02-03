using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataResolver
    {
        Maybe<TMetadata> Resolve<TMetadata, TSubject>(IMetadataDeclaration<TMetadata> declaration, Maybe<TSubject> subject, MemberInfo memberInfo);
    }
}
