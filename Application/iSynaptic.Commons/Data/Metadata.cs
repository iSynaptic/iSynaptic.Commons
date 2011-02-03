using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public static class Metadata
    {
        public static TMetadata Get<TMetadata>()
        {
            return Resolve(MetadataDeclaration<TMetadata>.TypeDeclaration, Maybe<object>.NoValue, null);
        }

        public static TMetadata Resolve<TMetadata, TSubject>(IMetadataDeclaration<TMetadata> declaration, Maybe<TSubject> subject, Expression member)
        {
            Guard.NotNull(declaration, "declaration");

            MemberInfo memberInfo = null;
            
            if(member != null)
                memberInfo = member.ExtractMemberInfoForMetadata();

            var resolver = MetadataResolver;

            if (resolver == null)
                resolver = Ioc.Resolve<IMetadataResolver>();

            return declaration.Resolve(resolver, subject, memberInfo);
        }
        
        public static void SetResolver(IMetadataResolver metadataResolver)
        {
            MetadataResolver = metadataResolver;
        }

        private static IMetadataResolver MetadataResolver { get; set; }
    }
}
