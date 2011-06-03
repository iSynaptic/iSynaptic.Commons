using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    internal class ExodataResolutionRoot<TExodata, TContext> : IExodataResolutionRoot<TExodata>, IExodataResolutionSubject<TExodata>
    {
        private readonly Maybe<TContext> _Context;
        private readonly IExodataDeclaration<TExodata> _Declaration;

        public ExodataResolutionRoot(IExodataDeclaration<TExodata> declaration, Maybe<TContext> context = default(Maybe<TContext>))
        {
            _Declaration = Guard.NotNull(declaration, "declaration");
            _Context = context;
        }

        public IExodataResolutionSubject<TExodata> Given<TDesiredContext>()
        {
            return new ExodataResolutionRoot<TExodata, TDesiredContext>(_Declaration);
        }

        public IExodataResolutionSubject<TExodata> Given<TDesiredContext>(TDesiredContext context)
        {
            return new ExodataResolutionRoot<TExodata, TDesiredContext>(_Declaration, context);
        }

        public TExodata Get()
        {
            return _Declaration.Resolve(_Context, Maybe<object>.NoValue, (MemberInfo)null);
        }

        public TExodata For<TSubject>()
        {
            return Resolve(_Context, Maybe<TSubject>.NoValue, null);
        }

        public TExodata For<TSubject>(TSubject subject)
        {
            return Resolve(_Context, subject.ToMaybe(), null);
        }

        public TExodata For<TSubject>(Expression<Func<TSubject, object>> member)
        {
            return Resolve(_Context, Maybe<TSubject>.NoValue, member);
        }

        public TExodata For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            return Resolve(_Context, subject.ToMaybe(), member);
        }

        public LazyExodata<TExodata> LazyGet()
        {
            return new LazyExodata<TExodata>(Get);
        }

        public LazyExodata<TExodata> LazyFor<TSubject>()
        {
            return new LazyExodata<TExodata>(For<TSubject>);
        }

        public LazyExodata<TExodata> LazyFor<TSubject>(TSubject subject)
        {
            return new LazyExodata<TExodata>(() => For(subject));
        }

        public LazyExodata<TExodata> LazyFor<TSubject>(Expression<Func<TSubject, object>> member)
        {
            return new LazyExodata<TExodata>(() => For(member));
        }

        public LazyExodata<TExodata> LazyFor<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            return new LazyExodata<TExodata>(() => For(subject, member));
        }

        protected TExodata Resolve<TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, Expression member)
        {
            MemberInfo memberInfo = member != null 
                ? member.ExtractMemberInfoForExodata<TSubject>()
                : null;

            return _Declaration.Resolve(context, subject, memberInfo);
        }
    }
}
