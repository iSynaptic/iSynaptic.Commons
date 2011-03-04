using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    internal class FluentExodataBindingBuilder<TExodata, TSubject> : BaseFluentExodataBindingBuilder<TExodata, TSubject>, IFluentExodataBindingSpecificSubjectPredicateScopeTo<TExodata, TSubject>
    {
        public FluentExodataBindingBuilder(IExodataBindingSource source, IExodataDeclaration declaration, Action<IExodataBinding> onBuildComplete)
            : base(source, declaration, onBuildComplete)
        {
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For(Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");
            Member = member.ExtractMemberInfoForExodata();

            return this;
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For(TSubject subject)
        {
            Subject = subject;

            return this;
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");

            Subject = subject;
            Member = member.ExtractMemberInfoForExodata();

            return this;
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, TDerivedSubject> For<TDerivedSubject>() where TDerivedSubject : TSubject
        {
            return UseDerivedSubject<TDerivedSubject>();
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, TDerivedSubject> For<TDerivedSubject>(Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject
        {
            return UseDerivedSubject<TDerivedSubject>()
                .For(member);
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject) where TDerivedSubject : TSubject
        {
            return UseDerivedSubject<TDerivedSubject>()
                .For(subject);
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject, Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject
        {
            return UseDerivedSubject<TDerivedSubject>()
                .For(subject, member);
        }

        private IFluentExodataBindingSpecificSubjectPredicateScopeTo<TExodata, TDerivedSubject> UseDerivedSubject<TDerivedSubject>()
        {
            return new FluentExodataBindingBuilder<TExodata, TDerivedSubject>(Source, Declaration, OnBuildComplete);
        }

    }

    internal class FluentExodataBindingBuilder<TExodata> : BaseFluentExodataBindingBuilder<TExodata, object>, IFluentExodataBindingSubjectPredicateScopeTo<TExodata>
    {
        public FluentExodataBindingBuilder(IExodataBindingSource source, IExodataDeclaration declaration, Action<IExodataBinding> onBuildComplete)
            : base(source, declaration, onBuildComplete)
        {
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For<TSubject>()
        {
            return UseSpecificSubject<TSubject>();
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For<TSubject>(Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");

            return UseSpecificSubject<TSubject>()
                .For(member);
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For<TSubject>(TSubject subject)
        {
            return UseSpecificSubject<TSubject>()
                .For(subject);
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");

            return UseSpecificSubject<TSubject>()
                .For(subject, member);
        }

        private IFluentExodataBindingSpecificSubjectPredicateScopeTo<TExodata, TSubject> UseSpecificSubject<TSubject>()
        {
            return new FluentExodataBindingBuilder<TExodata, TSubject>(Source, Declaration, OnBuildComplete);
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, object> For(Expression<Func<object, object>> member)
        {
            Guard.NotNull(member, "member");
            return ForCore(Maybe<object>.NoValue, member);
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, object> For(object subject)
        {
            return ForCore(subject, null);
        }

        public IFluentExodataBindingPredicateScopeTo<TExodata, object> For(object subject, Expression<Func<object, object>> member)
        {            
            Guard.NotNull(member, "member");
            return ForCore(subject, member);
        }

        private IFluentExodataBindingPredicateScopeTo<TExodata, object> ForCore(Maybe<object> subject, Expression<Func<object, object>> member)
        {
            Subject = subject;

            if(member != null)
                Member = member.ExtractMemberInfoForExodata();

            return this;
        }
    }
}


