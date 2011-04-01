using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class ExodataRequest<TSubject> : IExodataRequest<TSubject>, IEquatable<IExodataRequest<TSubject>>
    {
        public ExodataRequest(IExodataDeclaration declaration, IMaybe<TSubject> subject, MemberInfo member)
        {
            Declaration = Guard.NotNull(declaration, "declaration");
            Subject = subject;
            Member = member;
        }

        public IExodataDeclaration Declaration { get; private set; }

        public IMaybe<TSubject> Subject { get; private set; }
        public MemberInfo Member { get; private set; }

        public bool Equals(IExodataRequest<TSubject> other)
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
                Equals(obj as ExodataRequest<TSubject>);
        }

        public override int GetHashCode()
        {
            int results = 42;

            results = results ^ Declaration.GetHashCode();
            results = results ^ (Subject != null ? Subject.GetHashCode() : 0);
            results = results ^ (Member != null ? Member.GetHashCode() : 0);

            return results;
        }
    }
}
