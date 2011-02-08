using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    internal class MetadataRequest<TMetadata, TSubject> : IMetadataRequest<TMetadata, TSubject>
    {
        public MetadataRequest(IMetadataDeclaration declaration, IMaybe<TSubject> subject, MemberInfo member)
        {
            Guard.NotNull(declaration, "declaration");

            Declaration = declaration;
            Subject = subject;
            Member = member;
        }

        public IMetadataDeclaration Declaration { get; private set; }

        public IMaybe<TSubject> Subject { get; private set; }
        public MemberInfo Member { get; private set; }
   }
}
