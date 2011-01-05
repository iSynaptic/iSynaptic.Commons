using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class Metadata
    {
        protected Metadata()
        {
            throw new NotSupportedException();
        }

        public static TMetadata Get<TMetadata>(MetadataDeclaration<TMetadata> declaration)
        {
            return Metadata<object>.Get(declaration, null, null);
        }

        public static void SetMetadataResolver(IMetadataResolver metadataResolver)
        {
            MetadataResolver = metadataResolver;
        }

        protected static IMetadataResolver MetadataResolver { get; private set; }
    }

    public class Metadata<T> : Metadata
    {
        protected Metadata()
        {
        }

        public static new TMetadata Get<TMetadata>(MetadataDeclaration<TMetadata> declaration)
        {
            return Resolve(declaration, typeof(T), null);
        }

        public static TMetadata Get<TMetadata>(MetadataDeclaration<TMetadata> declaration, T subject)
        {
            return Resolve(declaration, subject, null);
        }

        public static TMetadata Get<TMetadata>(MetadataDeclaration<TMetadata> declaration, MemberInfo member)
        {
            return Resolve(declaration, typeof(T), member);
        }

        public static TMetadata Get<TMetadata>(MetadataDeclaration<TMetadata> declaration, T subject, MemberInfo member)
        {
            return Resolve(declaration, subject, member);
        }

        private static TMetadata Resolve<TMetadata>(MetadataDeclaration<TMetadata> declaration, object subject, MemberInfo member)
        {
            Guard.NotNull(declaration, "declaration");

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

            return resolver.Resolve(declaration, subject, member);
        }
    }
}
