using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataRequest<TMetadata>
    {
        public MetadataRequest(IMetadataDeclaration<TMetadata> declaration, object subject, MemberInfo member)
        {
            if(declaration == null)
                throw new ArgumentNullException("declaration");

            Declaration = declaration;
            Subject = subject;
            Member = member;
        }

        public IMetadataDeclaration<TMetadata> Declaration { get; private set; }

        public object Subject { get; private set; }
        public MemberInfo Member { get; private set; }
   }
}
