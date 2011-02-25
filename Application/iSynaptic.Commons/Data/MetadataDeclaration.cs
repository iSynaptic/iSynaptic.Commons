using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataDeclaration<TMetadata> : MetadataDeclaration, IMetadataDeclaration<TMetadata>
    {
        public static readonly MetadataDeclaration<TMetadata> TypeDeclaration = new MetadataDeclaration<TMetadata>();

        private Maybe<TMetadata> _Default = Maybe<TMetadata>.NoValue;

        public MetadataDeclaration()
        {
        }

        public MetadataDeclaration(TMetadata @default) : this()
        {
            _Default = new Maybe<TMetadata>(@default);
        }

        #region Get/For Methods

        public TMetadata Get()
        {
            return Resolve(Maybe<object>.NoValue, (MemberInfo)null);
        }

        public TMetadata For<TSubject>()
        {
            return Resolve(Maybe<TSubject>.NoValue, (MemberInfo)null);
        }

        public TMetadata For<TSubject>(TSubject subject)
        {
            return Resolve(new Maybe<TSubject>(subject), (MemberInfo)null);
        }

        public TMetadata For<TSubject>(Expression<Func<TSubject, object>> member)
        {
            return Resolve(Maybe<TSubject>.NoValue, member);
        }

        public TMetadata For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            return Resolve(new Maybe<TSubject>(subject), member);
        }

        #endregion

        #region Lazy Get/For Methods

        public LazyMetadata<TMetadata> LazyGet()
        {
            return new LazyMetadata<TMetadata>(this);
        }

        public LazyMetadata<TMetadata, TSubject> LazyFor<TSubject>()
        {
            return new LazyMetadata<TMetadata, TSubject>(this);
        }

        public LazyMetadata<TMetadata, TSubject> LazyFor<TSubject>(TSubject subject)
        {
            return new LazyMetadata<TMetadata, TSubject>(this, subject);
        }

        public LazyMetadata<TMetadata, TSubject> LazyFor<TSubject>(Expression<Func<TSubject, object>> member)
        {
            return new LazyMetadata<TMetadata, TSubject>(this, GetMemberInfoFromExpression(member));
        }

        public LazyMetadata<TMetadata, TSubject> LazyFor<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            return new LazyMetadata<TMetadata, TSubject>(this, subject, GetMemberInfoFromExpression(member));
        }

        #endregion

        #region Resolution Logic

        public TMetadata Resolve<TSubject>(Maybe<TSubject> subject, MemberInfo member)
        {
            var request = new MetadataRequest<TSubject>(this, subject, member);

            var resolvedResult = TryResolve(request);

            if (resolvedResult.HasValue)
            {
                OnValidateValue(resolvedResult.Value, "bound");
                return resolvedResult.Value;
            }

            var @default = GetDefault(request);
            OnValidateValue(@default, "default");

            return @default;
        }

        protected TMetadata Resolve<TSubject>(Maybe<TSubject> subject, Expression member)
        {
            MemberInfo memberInfo = GetMemberInfoFromExpression(member);

            return Resolve(subject, memberInfo);
        }

        protected virtual Maybe<TMetadata> TryResolve<TSubject>(IMetadataRequest<TSubject> request)
        {
            var resolver = MetadataResolver ?? Ioc.Resolve<IMetadataResolver>();

            return resolver != null
                       ? resolver.Resolve<TMetadata, TSubject>(request)
                       : Maybe<TMetadata>.NoValue;
        }

        #endregion

        protected virtual MemberInfo GetMemberInfoFromExpression(Expression member)
        {
            MemberInfo memberInfo = null;

            if (member != null)
                memberInfo = member.ExtractMemberInfoForMetadata();

            return memberInfo;
        }

        protected virtual void OnValidateValue(TMetadata value, string valueName)
        {
        }

        public static implicit operator TMetadata(MetadataDeclaration<TMetadata> declaration)
        {
            return declaration.Get();
        }

        protected virtual TMetadata GetDefault<TSubject>(IMetadataRequest<TSubject> request)
        {
            if (_Default.HasValue)
                return _Default.Value;

            return default(TMetadata);
        }

        public TMetadata Default
        {
            get
            {
                TMetadata defaultValue = GetDefault(new MetadataRequest<object>(this, Maybe<object>.NoValue, null));

                OnValidateValue(defaultValue, "default");

                return defaultValue;
            }
        }
    }

    public abstract class MetadataDeclaration : IMetadataDeclaration
    {
        protected internal MetadataDeclaration()
        {
        }

        public static TMetadata Get<TMetadata>()
        {
            return MetadataDeclaration<TMetadata>.TypeDeclaration.Get();
        }

        public static void SetResolver(IMetadataResolver metadataResolver)
        {
            MetadataResolver = metadataResolver;
        }

        protected static IMetadataResolver MetadataResolver { get; set; }
    }
}
