using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    internal abstract class BaseFluentMetadataBindingBuilder<TMetadata> : IPredicateScopeToBinding<TMetadata>
    {
        protected BaseFluentMetadataBindingBuilder(IMetadataBindingSource source, Func<MetadataRequest<TMetadata>, bool> userPredicate, Action<MetadataBinding<TMetadata>> onCompletedBinding)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(userPredicate, "userPredicate");
            Guard.NotNull(onCompletedBinding, "onCompletedBinding");

            Source = source;
            UserPredicate = userPredicate;
            OnCompletedBinding = onCompletedBinding;
        }

        
        public void To(TMetadata value)
        {
            To(r => value);
        }

        public void To(Func<MetadataRequest<TMetadata>, TMetadata> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");
            OnCompletedBinding(new MetadataBinding<TMetadata>(Matches, valueFactory, Source)
                                   {
                                       ScopeFactory = ScopeFactory,
                                       Subject = Subject,
                                       Member = Member
                                   });
        }

        public IScopeToBinding<TMetadata> When(Func<MetadataRequest<TMetadata>, bool> userPredicate)
        {
            Guard.NotNull(userPredicate, "userPredicate");
            UserPredicate = userPredicate;
            return this;
        }

        public IToBinding<TMetadata> InScope(object scopeObject)
        {
            Guard.NotNull(scopeObject, "scopeObject");
            ScopeFactory = r => scopeObject;
            return this;
        }

        public IToBinding<TMetadata> InScope(Func<MetadataRequest<TMetadata>, object> scopeFactory)
        {
            Guard.NotNull(scopeFactory, "scopeFactory");
            ScopeFactory = scopeFactory;
            return this;
        }

        private bool Matches(MetadataRequest<TMetadata> request)
        {
            Guard.NotNull(request, "request");

            if (Member != request.Member)
                return false;

            if ((Subject == null) != (request.Subject == null))
                return false;

            if (Subject != null && request.Subject != Subject)
            {
                if (Subject is Type != true)
                    return false;
                
                if (((Type)Subject).IsAssignableFrom(request.Subject.GetType()) != true)
                    return false;
            }

            return UserPredicate(request);
        }

        protected IMetadataBindingSource Source { get; set; }
        protected object Subject { get; set; }
        protected MemberInfo Member { get; set; }

        protected Func<MetadataRequest<TMetadata>, bool> UserPredicate { get; set; }
        protected Func<MetadataRequest<TMetadata>, object> ScopeFactory { get; set; }

        protected Action<MetadataBinding<TMetadata>> OnCompletedBinding { get; set; }

    }

    internal class FluentMetadataBindingBuilder<TMetadata, TSubject> : BaseFluentMetadataBindingBuilder<TMetadata>, ISpecificSubjectPredicateScopeToBinding<TMetadata, TSubject>
    {
        public FluentMetadataBindingBuilder(IMetadataBindingSource source, Func<MetadataRequest<TMetadata>, bool> userPredicate, Action<MetadataBinding<TMetadata>> onCompletedBinding)
            : base(source, userPredicate, onCompletedBinding)
        {
            Subject = typeof(TSubject);
        }

        public IPredicateScopeToBinding<TMetadata> For(Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");
            Member = member.ExtractMemberInfoForMetadata();

            return this;
        }

        public IPredicateScopeToBinding<TMetadata> For(TSubject subject)
        {
            Guard.NotNull(subject, "subject");
            Subject = subject;

            return this;
        }

        public IPredicateScopeToBinding<TMetadata> For(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(subject, "subject");
            Guard.NotNull(member, "member");

            Subject = subject;
            Member = member.ExtractMemberInfoForMetadata();

            return this;
        }
    }

    internal class FluentMetadataBindingBuilder<TMetadata> : BaseFluentMetadataBindingBuilder<TMetadata>, ISubjectPredicateScopeToBinding<TMetadata>
    {
        public FluentMetadataBindingBuilder(IMetadataBindingSource source, Func<MetadataRequest<TMetadata>, bool> userPredicate, Action<MetadataBinding<TMetadata>> onCompletedBinding)
            : base(source, userPredicate, onCompletedBinding)
        {
        }

        public IPredicateScopeToBinding<TMetadata> For<TSubject>()
        {
            Subject = typeof(TSubject);
            return this;
        }

        public IPredicateScopeToBinding<TMetadata> For<TSubject>(Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");

            Subject = typeof(TSubject);
            Member = member.ExtractMemberInfoForMetadata();
            return this;
        }

        public IPredicateScopeToBinding<TMetadata> For<TSubject>(TSubject subject)
        {
            Guard.NotNull(subject, "subject");

            Subject = subject;
            return this;
        }

        public IPredicateScopeToBinding<TMetadata> For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(subject, "subject");
            Guard.NotNull(member, "member");

            Subject = subject;
            Member = member.ExtractMemberInfoForMetadata();

            return this;
        }
    }
}
