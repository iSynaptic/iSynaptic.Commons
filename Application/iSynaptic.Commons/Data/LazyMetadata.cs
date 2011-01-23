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
            : base(Metadata.Get<TMetadata>)
        {
        }

        public LazyMetadata(IMetadataDeclaration<TMetadata> declaration)
            : base(() => Metadata.Resolve(declaration, Maybe<object>.NoValue, null))
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
        public LazyMetadata(IMetadataDeclaration<TMetadata> declaration) : base(() => Metadata.Resolve(declaration, Maybe<TSubject>.NoValue, null))
        {
        }

        public LazyMetadata(IMetadataDeclaration<TMetadata> declaration, TSubject subject) : base(() => Metadata.Resolve(declaration, new Maybe<TSubject>(subject), null))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public LazyMetadata(IMetadataDeclaration<TMetadata> declaration, Expression<Func<TSubject, object>> member) : base(() => Metadata.Resolve(declaration, Maybe<TSubject>.NoValue, member))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public LazyMetadata(IMetadataDeclaration<TMetadata> declaration, TSubject subject, Expression<Func<TSubject, object>> member) : base(() => Metadata.Resolve(declaration, new Maybe<TSubject>(subject), member))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public static implicit operator TMetadata(LazyMetadata<TMetadata, TSubject> value)
        {
            return value.Value;
        }
    }
}
