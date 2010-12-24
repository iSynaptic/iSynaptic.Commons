using System;
using System.Linq.Expressions;
using System.Reflection;

namespace iSynaptic.Commons.Data
{
    public static class MetadataExtensions
    {
        public static TMetadata For<T, TMetadata>(this MetadataDeclaration<TMetadata> declaration, T subject)
        {
            return Metadata<T>.Get<object, TMetadata>(declaration, subject, null);
        }

        public static TMetadata For<T, TMember, TMetadata>(this MetadataDeclaration<TMetadata> declaration, T subject, Expression<Func<T, TMember>> member)
        {
            return Metadata<T>.Get(declaration, subject, member);
        }
    }
}