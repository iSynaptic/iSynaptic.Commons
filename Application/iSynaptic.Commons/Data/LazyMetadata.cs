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
            : this(MetadataDeclaration<TMetadata>.TypeDeclaration)
        {
        }

        public LazyMetadata(MetadataDeclaration<TMetadata> declaration)
            : base(declaration.Get)
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
        public LazyMetadata(MetadataDeclaration<TMetadata> declaration) : base(() => declaration.For<TSubject>())
        {
        }

        public LazyMetadata(MetadataDeclaration<TMetadata> declaration, TSubject subject) : base(() => declaration.For(subject))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public LazyMetadata(MetadataDeclaration<TMetadata> declaration, Expression<Func<TSubject, object>> member) : base(() => declaration.For(member))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public LazyMetadata(MetadataDeclaration<TMetadata> declaration, TSubject subject, Expression<Func<TSubject, object>> member) : base(() => declaration.For(subject, member))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public static implicit operator TMetadata(LazyMetadata<TMetadata, TSubject> value)
        {
            return value.Value;
        }
    }
}
