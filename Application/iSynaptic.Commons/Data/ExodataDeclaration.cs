using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class ExodataDeclaration<TExodata> : ExodataDeclaration, IExodataDeclaration<TExodata>
    {
        public static readonly ExodataDeclaration<TExodata> TypeDeclaration = new ExodataDeclaration<TExodata>();

        private Maybe<TExodata> _Default = Maybe<TExodata>.NoValue;

        public ExodataDeclaration()
        {
        }

        public ExodataDeclaration(TExodata @default) : this()
        {
            _Default = Maybe.Value(@default);
        }

        #region Get/For Methods

        public TExodata Get()
        {
            return Resolve(Maybe<object>.NoValue, (MemberInfo)null);
        }

        public TExodata For<TSubject>()
        {
            return Resolve(Maybe<TSubject>.NoValue, (MemberInfo)null);
        }

        public TExodata For<TSubject>(TSubject subject)
        {
            return Resolve(new Maybe<TSubject>(subject), (MemberInfo)null);
        }

        public TExodata For<TSubject>(Expression<Func<TSubject, object>> member)
        {
            return Resolve(Maybe<TSubject>.NoValue, member);
        }

        public TExodata For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            return Resolve(new Maybe<TSubject>(subject), member);
        }

        #endregion

        #region Lazy Get/For Methods

        public LazyExodata<TExodata> LazyGet()
        {
            return new LazyExodata<TExodata>(this);
        }

        public LazyExodata<TExodata, TSubject> LazyFor<TSubject>()
        {
            return new LazyExodata<TExodata, TSubject>(this);
        }

        public LazyExodata<TExodata, TSubject> LazyFor<TSubject>(TSubject subject)
        {
            return new LazyExodata<TExodata, TSubject>(this, subject);
        }

        public LazyExodata<TExodata, TSubject> LazyFor<TSubject>(Expression<Func<TSubject, object>> member)
        {
            return new LazyExodata<TExodata, TSubject>(this, member.ExtractMemberInfoForExodata<TSubject>());
        }

        public LazyExodata<TExodata, TSubject> LazyFor<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            return new LazyExodata<TExodata, TSubject>(this, subject, member.ExtractMemberInfoForExodata<TSubject>());
        }

        #endregion

        #region Resolution Logic

        public TExodata Resolve<TSubject>(Maybe<TSubject> subject, MemberInfo member)
        {
            var request = new ExodataRequest<TSubject>(this, subject, member);

            return Maybe.Value(request)
                .Select(TryResolve)
                .Do(x => OnValidateValue(x, "bound"))
                .ThrowOnException()
                .OnNoValue(() => GetDefault(request))
                .Do(x => OnValidateValue(x, "default"))
                .Value;
        }

        protected TExodata Resolve<TSubject>(Maybe<TSubject> subject, Expression member)
        {
            MemberInfo memberInfo = member.ExtractMemberInfoForExodata<TSubject>();

            return Resolve(subject, memberInfo);
        }

        protected virtual Maybe<TExodata> TryResolve<TSubject>(IExodataRequest<TSubject> request)
        {
            return Maybe
                .Value(ExodataResolver).NotNull()
                .OnNoValue(() => Ioc.Resolve<IExodataResolver>()).NotNull()
                .Select(x => x.Resolve<TExodata, TSubject>(request));
        }

        #endregion

        protected virtual void OnValidateValue(TExodata value, string valueName)
        {
        }

        public static implicit operator TExodata(ExodataDeclaration<TExodata> declaration)
        {
            return declaration.Get();
        }

        protected virtual TExodata GetDefault<TSubject>(IExodataRequest<TSubject> request)
        {
            return _Default.Return();
        }

        public TExodata Default
        {
            get
            {
                TExodata defaultValue = GetDefault(new ExodataRequest<object>(this, Maybe<object>.NoValue, null));

                OnValidateValue(defaultValue, "default");

                return defaultValue;
            }
        }
    }

    public abstract class ExodataDeclaration : IExodataDeclaration
    {
        protected internal ExodataDeclaration()
        {
        }

        public static TExodata Get<TExodata>()
        {
            return ExodataDeclaration<TExodata>.TypeDeclaration.Get();
        }

        public static void SetResolver(IExodataResolver exodataResolver)
        {
            ExodataResolver = exodataResolver;
        }

        protected static IExodataResolver ExodataResolver { get; set; }
    }
}
