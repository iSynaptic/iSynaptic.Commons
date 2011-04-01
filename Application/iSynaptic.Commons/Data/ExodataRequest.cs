using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class ExodataRequest<TContext, TSubject> : IExodataRequest<TContext, TSubject>, IEquatable<IExodataRequest<TContext, TSubject>>
    {
        public ExodataRequest(IExodataDeclaration declaration, IMaybe<TContext> context, IMaybe<TSubject> subject, MemberInfo member)
        {
            Declaration = Guard.NotNull(declaration, "declaration");
            Context = context;
            Subject = subject;
            Member = member;
        }

        public IExodataDeclaration Declaration { get; private set; }

        public IMaybe<TContext> Context { get; private set; }
        public IMaybe<TSubject> Subject { get; private set; }
        public MemberInfo Member { get; private set; }

        public bool Equals(IExodataRequest<TContext, TSubject> other)
        {
            return Maybe
                .Value(other)
                .NotNull()
                .Unless(x => Declaration != x.Declaration)
                .Unless(x => ReferenceEquals(Subject, null) != ReferenceEquals(x.Subject, null))
                .Unless(x => !ReferenceEquals(Subject, null) && !Subject.Equals(x.Subject))
                .Unless(x => Member != x.Member)
                .Select(x => true)
                .ThrowOnException()
                .Return(false);
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(obj, null) &&
                Equals(obj as ExodataRequest<TContext, TSubject>);
        }

        public override int GetHashCode()
        {
            int results = 42;

            results = results ^ Declaration.GetHashCode();
            results = results ^ (Context != null ? Context.GetHashCode() : 0);
            results = results ^ (Subject != null ? Subject.GetHashCode() : 0);
            results = results ^ (Member != null ? Member.GetHashCode() : 0);

            return results;
        }
    }
}
