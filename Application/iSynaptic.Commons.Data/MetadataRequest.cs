using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataRequest<TMetadata, TSubject>
    {
        public MetadataRequest(MetadataDeclaration<TMetadata> declaration, Maybe<TSubject> subject, MemberInfo member)
        {
            Guard.NotNull(declaration, "declaration");

            Declaration = declaration;
            Subject = subject;
            Member = member;
        }

        public MetadataDeclaration<TMetadata> Declaration { get; private set; }

        public Maybe<TSubject> Subject { get; private set; }
        public MemberInfo Member { get; private set; }
   }
}
