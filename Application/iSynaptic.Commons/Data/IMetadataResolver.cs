using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataResolver
    {
        T Resolve<T>(MetadataDeclaration<T> declaration, object subject, MemberInfo memberInfo);
    }
}
