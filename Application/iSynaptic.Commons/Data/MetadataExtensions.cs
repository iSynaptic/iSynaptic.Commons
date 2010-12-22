using System.Reflection;

namespace iSynaptic.Commons.Data
{
    public static class MetadataExtensions
    {
        public static TMetadata For<T, TMetadata>(this MetadataDeclaration<TMetadata> declaration, T subject)
        {
            return Metadata<T>.Get(declaration, subject, null);
        }

        public static TMetadata For<T, TMetadata>(this MetadataDeclaration<TMetadata> declaration, T subject, MemberInfo member)
        {
            return Metadata<T>.Get(declaration, subject, member);
        }
    }
}