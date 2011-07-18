using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public static class ExodataResolverExtensions
    {
        public static IFluentExodataResolutionRoot<TExodata> TryResolve<TExodata>(this IExodataResolver resolver, ISymbol<TExodata> symbol)
        {
            Guard.NotNull(resolver, "resolver");
            Guard.NotNull(symbol, "symbol");

            return new ExodataResolverFluentExodataResolutionRoot<TExodata, object>(resolver, symbol);
        }
    }

    internal class ExodataResolverFluentExodataResolutionRoot<TExodata, TContext> : FluentExodataResolutionRoot<TExodata, TContext>
    {
        private readonly ISymbol<TExodata> _Symbol;
        private readonly IExodataResolver _Resolver;

        public ExodataResolverFluentExodataResolutionRoot(IExodataResolver resolver, ISymbol<TExodata> symbol, Maybe<TContext> context = default(Maybe<TContext>))
            : base(context)
        {
            _Resolver = Guard.NotNull(resolver, "resolver");
            _Symbol = Guard.NotNull(symbol, "symbol");
        }

        protected override IFluentExodataResolutionRoot<TExodata> CreateNewResolutionRoot<TDesiredContext>(Maybe<TDesiredContext> context)
        {
            return new ExodataResolverFluentExodataResolutionRoot<TExodata, TDesiredContext>(_Resolver, _Symbol, context);
        }

        protected override TExodata Resolve<TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            return _Resolver.TryResolve<TExodata, TContext, TSubject>(_Symbol, context, subject, member).Value;
        }

        protected override Maybe<TExodata> TryResolve<TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            return _Resolver.TryResolve<TExodata, TContext, TSubject>(_Symbol, context, subject, member);
        }
    }

}
