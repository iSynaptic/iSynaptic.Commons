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
        //public static TMetadata Get<TMetadata>(MetadataDeclaration<TMetadata> declaration)
        //{
        //    return Resolve(declaration, null, null);
        //}

        public static TMetadata Resolve<TMetadata>(MetadataDeclaration<TMetadata> declaration, object subject, Expression member)
        {
            Guard.NotNull(declaration, "declaration");

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
            if (lambda != null)
            {
                var body = lambda.Body;

                if (body.NodeType == ExpressionType.Convert)
                    body = ((UnaryExpression) body).Operand;

                if (body is MemberExpression)
                {
                    var memberExpression = (MemberExpression) body;

                    if (memberExpression.Expression is ParameterExpression && memberExpression.Member is PropertyInfo ||
                        memberExpression.Member is FieldInfo)
                        return memberExpression.Member;
                }
            }

            throw new ArgumentException("You can only retreive member metatdata for properties and fields.", "member");
        }

        public static void SetResolver(IMetadataResolver metadataResolver)
        {
            MetadataResolver = metadataResolver;
        }

        private static IMetadataResolver MetadataResolver { get; set; }
    }

    //public class Metadata<T> : Metadata
    //{
    //    public static new TMetadata Get<TMetadata>(MetadataDeclaration<TMetadata> declaration)
    //    {
    //        return Resolve(declaration, typeof(T), null);
    //    }

    //    public static TMetadata Get<TMetadata>(MetadataDeclaration<TMetadata> declaration, T subject)
    //    {
    //        return Resolve(declaration, subject, null);
    //    }

    //    public static TMetadata Get<TMember, TMetadata>(MetadataDeclaration<TMetadata> declaration, Expression<Func<T, TMember>> member)
    //    {
    //        return Resolve(declaration, typeof(T), member);
    //    }

    //    public static TMetadata Get<TMember, TMetadata>(MetadataDeclaration<TMetadata> declaration, T subject, Expression<Func<T, TMember>> member)
    //    {
    //        return Resolve(declaration, subject, member);
    //    }
    //}
}
