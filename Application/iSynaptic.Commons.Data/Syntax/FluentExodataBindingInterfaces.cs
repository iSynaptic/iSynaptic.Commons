using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentExodataBindingRoot<TContextBase, TSubjectBase>
    {
        IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, TContextBase, TSubjectBase> Bind<TExodata>(ISymbol<TExodata> symbol);
        void Bind<TExodata>(ISymbol<TExodata> symbol, TExodata value, string name = null);
    }

    public interface IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, TContextBase, TSubjectBase> : IFluentExodataBindingGivenSubjectWhenTo<TExodata, TContextBase, TSubjectBase>
    {
        IFluentExodataBindingGivenSubjectWhenTo<TExodata, TContextBase, TSubjectBase> Named(string name);
    }

    public interface IFluentExodataBindingGivenSubjectWhenTo<TExodata, TContextBase, TSubjectBase> : IFluentExodataBindingSubjectWhenTo<TExodata, TContextBase, TSubjectBase>
    {
        IFluentExodataBindingSubjectWhenTo<TExodata, TContext, TSubjectBase> Given<TContext>() where TContext : TContextBase;
        IFluentExodataBindingSubjectWhenTo<TExodata, TContext, TSubjectBase> Given<TContext>(TContext context) where TContext : TContextBase;
    }

    public interface IFluentExodataBindingSubjectWhenTo<TExodata, TContext, TSubjectBase> : IFluentExodataBindingWhenTo<TExodata, TContext, TSubjectBase>
    {
        IFluentExodataBindingWhenTo<TExodata, TContext, TSubjectBase> For(TSubjectBase subject);
        IFluentExodataBindingWhenTo<TExodata, TContext, TSubjectBase> For(params Expression<Func<TSubjectBase, object>>[] members);
        IFluentExodataBindingWhenTo<TExodata, TContext, TSubjectBase> For(TSubjectBase subject, params Expression<Func<TSubjectBase, object>>[] members);

        IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> For<TSubject>() where TSubject : TSubjectBase;
        IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> For<TSubject>(TSubject subject) where TSubject : TSubjectBase;
        IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> For<TSubject>(params Expression<Func<TSubject, object>>[] members) where TSubject : TSubjectBase;
        IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> For<TSubject>(TSubject subject, params Expression<Func<TSubject, object>>[] members) where TSubject : TSubjectBase;
    }

    public interface IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> : IFluentExodataBindingTo<TExodata, TContext, TSubject>
    {
        IFluentExodataBindingTo<TExodata, TContext, TSubject> When(Func<IExodataRequest<TExodata, TContext, TSubject>, bool> predicate);
    }

    public interface IFluentExodataBindingTo<TExodata, TContext, TSubject> : IFluentInterface
    {
        void To(TExodata value);
        void To(Func<IExodataRequest<TExodata, TContext, TSubject>, Maybe<TExodata>> valueFactory);
    }
}
