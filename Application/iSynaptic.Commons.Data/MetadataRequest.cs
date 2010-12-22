using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataRequest
    {
        public MetadataRequest(IMetadataDeclaration declaration, object subject, MemberInfo member)
        {
            if(declaration == null)
                throw new ArgumentNullException("declaration");

            Declaration = declaration;
            Subject = subject;
            Member = member;
        }

        public IMetadataDeclaration Declaration { get; private set; }

        public object Subject { get; private set; }
        public MemberInfo Member { get; private set; }
   }
}
