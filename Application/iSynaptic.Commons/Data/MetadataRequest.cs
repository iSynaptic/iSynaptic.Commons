using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataRequest<TSubject> : IMetadataRequest<TSubject>, IEquatable<IMetadataRequest<TSubject>>
    {
        public MetadataRequest(IMetadataDeclaration declaration, IMaybe<TSubject> subject, MemberInfo member)
        {
            Guard.NotNull(declaration, "declaration");

            Declaration = declaration;
            Subject = subject;
            Member = member;
        }

        public IMetadataDeclaration Declaration { get; private set; }

        public IMaybe<TSubject> Subject { get; private set; }
        public MemberInfo Member { get; private set; }

        public bool Equals(IMetadataRequest<TSubject> other)
        {
            if (other == null)
                return false;

            if (Declaration != other.Declaration)
                return false;

            if (ReferenceEquals(Subject, null) != ReferenceEquals(other.Subject, null))
                return false;

            if (!ReferenceEquals(Subject, null) && !Subject.Equals(other.Subject))
                return false;

            if (Member != other.Member)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return Equals(obj as MetadataRequest<TSubject>);
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
