using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class LazyMetadata<TMetadata> : Lazy<TMetadata>
    {
        public LazyMetadata()
            : base(MetadataDeclaration.Get<TMetadata>)
        {
        }

        public LazyMetadata(IMetadataDeclaration<TMetadata> declaration)
            : base(() => declaration.Resolve(Maybe<object>.NoValue, null))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public static implicit operator TMetadata(LazyMetadata<TMetadata> value)
        {
            return value.Value;
        }
    }

    public class LazyMetadata<TMetadata, TSubject> : Lazy<TMetadata>
    {
        public LazyMetadata(IMetadataDeclaration<TMetadata> declaration)
            : base(() => declaration.Resolve(Maybe<TSubject>.NoValue, null))
        {
        }

        public LazyMetadata(IMetadataDeclaration<TMetadata> declaration, TSubject subject)
            : base(() => declaration.Resolve(new Maybe<TSubject>(subject), null))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public LazyMetadata(IMetadataDeclaration<TMetadata> declaration, MemberInfo member)
            : base(() => declaration.Resolve(Maybe<TSubject>.NoValue, member))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public LazyMetadata(IMetadataDeclaration<TMetadata> declaration, TSubject subject, MemberInfo member)
            : base(() => declaration.Resolve(new Maybe<TSubject>(subject), member))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public static implicit operator TMetadata(LazyMetadata<TMetadata, TSubject> value)
        {
            return value.Value;
        }
    }
}
