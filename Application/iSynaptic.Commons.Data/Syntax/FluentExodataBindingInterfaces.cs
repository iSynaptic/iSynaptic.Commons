using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentExodataBindingRoot<TContextBase, TSubjectBase>
    {
        IFluentExodataBindingGivenSubjectWhenScopeTo<TExodata, TContextBase, TSubjectBase> Bind<TExodata>(IExodataDeclaration<TExodata> declaration);
        void Bind<TExodata>(IExodataDeclaration<TExodata> declaration, TExodata value);
    }

    public interface IFluentExodataBindingGivenSubjectWhenScopeTo<TExodata, TContextBase, TSubjectBase> : IFluentExodataBindingSubjectWhenScopeTo<TExodata, TContextBase, TSubjectBase>
    {
        IFluentExodataBindingSubjectWhenScopeTo<TExodata, TContext, TSubjectBase> Given<TContext>() where TContext : TContextBase;
        IFluentExodataBindingSubjectWhenScopeTo<TExodata, TContext, TSubjectBase> Given<TContext>(TContext context) where TContext : TContextBase;
    }

    public interface IFluentExodataBindingSubjectWhenScopeTo<TExodata, TContext, TSubjectBase> : IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubjectBase>
    {
        IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubjectBase> For(TSubjectBase subject);
        IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubjectBase> For(params Expression<Func<TSubjectBase, object>>[] members);
        IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubjectBase> For(TSubjectBase subject, params Expression<Func<TSubjectBase, object>>[] members);

        IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubject> For<TSubject>() where TSubject : TSubjectBase;
        IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubject> For<TSubject>(TSubject subject) where TSubject : TSubjectBase;
        IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubject> For<TSubject>(params Expression<Func<TSubject, object>>[] members) where TSubject : TSubjectBase;
        IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubject> For<TSubject>(TSubject subject, params Expression<Func<TSubject, object>>[] members) where TSubject : TSubjectBase;
    }

    public interface IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubject> : IFluentExodataBindingScopeTo<TExodata, TContext, TSubject>
    {
        IFluentExodataBindingScopeTo<TExodata, TContext, TSubject> When(Func<IExodataRequest<TExodata, TContext, TSubject>, bool> predicate);
    }

    public interface IFluentExodataBindingScopeTo<TExodata, TContext, TSubject> : IFluentExodataBindingTo<TExodata, TContext, TSubject>
    {
        IFluentExodataBindingTo<TExodata, TContext, TSubject> InScope(object scopeObject);
        IFluentExodataBindingTo<TExodata, TContext, TSubject> InScope(Func<IExodataRequest<TExodata, TContext, TSubject>, object> scopeFactory);
    }

    public interface IFluentExodataBindingTo<TExodata, TContext, TSubject> : IFluentInterface
    {
        void To(TExodata value);
        void To(Func<IExodataRequest<TExodata, TContext, TSubject>, TExodata> valueFactory);
    }
}
