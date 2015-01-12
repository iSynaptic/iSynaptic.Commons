// The MIT License
// 
// Copyright (c) 2012-2015 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq.Expressions;
using System.Reflection;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public class ExodataDeclaration<TExodata> : ExodataDeclaration, ISymbol<TExodata>, IFluentExodataResolutionRoot<TExodata>
    {
        public static readonly ExodataDeclaration<TExodata> TypeDeclaration = new ExodataDeclaration<TExodata>();

        private Maybe<TExodata> _Default = Maybe<TExodata>.NoValue;

        public ExodataDeclaration()
        {
        }

        public ExodataDeclaration(TExodata @default) : this(@default.ToMaybe())
        {
            Guard.NotNull(@default, "@default");
        }

        public ExodataDeclaration(Maybe<TExodata> @default)
        {
            _Default = @default;
        }

        #region Fluent Resolution

        public IFluentExodataResolutionRoot<TExodata> Given<TContext>()
        {
            return new ExodataDeclarationResolutionRoot<TExodata, TContext>(this);
        }

        public IFluentExodataResolutionRoot<TExodata> Given<TContext>(TContext context)
        {
            return new ExodataDeclarationResolutionRoot<TExodata, TContext>(this, context.ToMaybe());
        }

        public Maybe<TExodata> TryGet()
        {
            return Given<object>().TryGet();
        }

        public Maybe<TExodata> TryFor<TSubject>()
        {
            return Given<object>().TryFor<TSubject>();
        }

        public Maybe<TExodata> TryFor<TSubject>(TSubject subject)
        {
            return Given<object>().TryFor(subject);
        }

        public Maybe<TExodata> TryFor<TSubject>(Expression<Func<TSubject, object>> member)
        {
            return Given<object>().TryFor(member);
        }

        public Maybe<TExodata> TryFor<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            return Given<object>().TryFor(subject, member);
        }

        public TExodata Get()
        {
            return Given<object>().Get();
        }

        public TExodata For<TSubject>()
        {
            return Given<object>().For<TSubject>();
        }

        public TExodata For<TSubject>(TSubject subject)
        {
            return Given<object>().For(subject);
        }

        public TExodata For<TSubject>(Expression<Func<TSubject, object>> member)
        {
            return Given<object>().For(member);
        }

        public TExodata For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            return Given<object>().For(subject, member);
        }

        #endregion

        #region Resolution Logic

        public virtual TExodata Resolve<TContext, TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            return TryResolve(context, subject, member)
                .Or(TryGetDefault(context, subject, member))
                .Value;
        }

        public virtual Maybe<TExodata> TryResolve<TContext, TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            return ExodataResolver
                .ToMaybe()
                .Or(Ioc.TryResolve<IExodataResolver>)
                .SelectMaybe(x => x.TryResolve(ExodataRequest.Create<TExodata, TContext, TSubject>(this, context, subject, member)))
                .SelectMaybe(x => EnsureValid(x, "bound"));
        }

        #endregion

        protected virtual Maybe<TExodata> EnsureValid(TExodata value, string valueName)
        {
            return value.ToMaybe();
        }

        public static implicit operator TExodata(ExodataDeclaration<TExodata> declaration)
        {
            return declaration.Get();
        }

        protected virtual Maybe<TExodata> TryGetDefault<TContext, TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            return _Default.SelectMaybe(x => EnsureValid(x, "default"));
        }
    }

    public abstract class ExodataDeclaration
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

    internal class ExodataDeclarationResolutionRoot<TExodata, TContext> : FluentExodataResolutionRoot<TExodata, TContext>
    {
        private readonly ExodataDeclaration<TExodata> _Declaration;
        public ExodataDeclarationResolutionRoot(ExodataDeclaration<TExodata> declaration, Maybe<TContext> context = default(Maybe<TContext>))
            : base(context)
        {
            _Declaration = Guard.NotNull(declaration, "declaration");
        }

        protected override IFluentExodataResolutionRoot<TExodata> CreateNewResolutionRoot<TDesiredContext>(Maybe<TDesiredContext> context)
        {
            return new ExodataDeclarationResolutionRoot<TExodata, TDesiredContext>(_Declaration, context);
        }

        protected override TExodata Resolve<TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            return _Declaration.Resolve(context, subject, member);
        }

        protected override Maybe<TExodata> TryResolve<TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            return _Declaration.TryResolve(context, subject, member);
        }
    }
}
