using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public static class ExodataRequest
    {
        public static ExodataRequest<TExodata, TContext, TSubject> Create<TExodata, TContext, TSubject>(ISymbol<TExodata> symbol, Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            return new ExodataRequest<TExodata, TContext, TSubject>(symbol, context, subject, member);
        }
    }

    public class ExodataRequest<TExodata, TContext, TSubject> : IExodataRequest<TExodata, TContext, TSubject>, IEquatable<IExodataRequest<TExodata, TContext, TSubject>>
    {
        public ExodataRequest(ISymbol<TExodata> symbol, Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            Symbol = Guard.NotNull(symbol, "symbol");
            Context = context;
            Subject = subject;
            Member = member;
        }

        public ISymbol<TExodata> Symbol { get; private set; }

        public Maybe<TContext> Context { get; private set; }
        public Maybe<TSubject> Subject { get; private set; }
        public MemberInfo Member { get; private set; }

        public bool Equals(IExodataRequest<TExodata, TContext, TSubject> other)
        {
            return Maybe
                .NotNull(other)
                .Unless(x => Symbol != x.Symbol)
                .Unless(x => ReferenceEquals(Subject, null) != ReferenceEquals(x.Subject, null))
                .Unless(x => !ReferenceEquals(Subject, null) && !Subject.Equals(x.Subject))
                .Unless(x => Member != x.Member)
                .Select(x => true)
                .ValueOrDefault(false);
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(obj, null) &&
                Equals(obj as ExodataRequest<TExodata, TContext, TSubject>);
        }

        public override int GetHashCode()
        {
            int results = 42;

            results = results ^ Symbol.GetHashCode();
            results = results ^ (Context != null ? Context.GetHashCode() : 0);
            results = results ^ (Subject != null ? Subject.GetHashCode() : 0);
            results = results ^ (Member != null ? Member.GetHashCode() : 0);

            return results;
        }
    }
}
