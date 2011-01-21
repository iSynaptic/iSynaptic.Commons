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
            return MetadataDeclaration<TMetadata>.TypeDeclaration.Get();
        }

        public static TMetadata Resolve<TMetadata, TSubject>(MetadataDeclaration<TMetadata> declaration, Maybe<TSubject> subject, Expression member)
        {
            Guard.NotNull(declaration, "declaration");

            MemberInfo memberInfo = null;
            
            if(member != null)
                memberInfo = member.ExtractMemberInfoForMetadata();

            var resolver = MetadataResolver;

            if (resolver == null)
                resolver = Ioc.Resolve<IMetadataResolver>();

            if (resolver == null)
            {
                try
                {
                    return declaration.Default;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Metadata resolver has not been set and obtaining the default value resulted in an exception. See inner exception(s) for details.", ex);
                }
            }

            return resolver.Resolve(declaration, subject, memberInfo);
        }
        
        public static void SetResolver(IMetadataResolver metadataResolver)
        {
            MetadataResolver = metadataResolver;
        }

        private static IMetadataResolver MetadataResolver { get; set; }
    }
}
