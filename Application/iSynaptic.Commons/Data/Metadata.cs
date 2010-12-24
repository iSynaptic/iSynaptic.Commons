using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            return Resolve(declaration, null, null);
        }

        protected static TMetadata Resolve<TMetadata>(MetadataDeclaration<TMetadata> declaration, object subject, Expression member)
        {
            if (declaration == null)
                throw new ArgumentNullException("declaration");

            MemberInfo memberInfo = null;
            
            if(member != null)
                memberInfo = ExtractMemberInfoFromExpression(member);

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

        private static MemberInfo ExtractMemberInfoFromExpression(Expression member)
        {
            var lambda = member as LambdaExpression;
            if(lambda != null && lambda.Body is MemberExpression)
            {
                var memberExpression = (MemberExpression) lambda.Body;

                if(memberExpression.Member is PropertyInfo || memberExpression.Member is FieldInfo)
                    return memberExpression.Member;
            }

            throw new ArgumentException("You can only retreive member metatdata for properties and fields.", "member");
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

        public static TMetadata Get<TMember, TMetadata>(MetadataDeclaration<TMetadata> declaration, Expression<Func<T, TMember>> member)
        {
            return Resolve(declaration, typeof(T), member);
        }

        public static TMetadata Get<TMember, TMetadata>(MetadataDeclaration<TMetadata> declaration, T subject, Expression<Func<T, TMember>> member)
        {
            return Resolve(declaration, subject, member);
        }
    }
}
