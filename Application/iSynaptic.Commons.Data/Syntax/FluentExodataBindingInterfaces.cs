using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentExodataBindingRoot<TContextBase, TSubjectBase>
    {
        IFluentExodataBindingSubjectGivenWhenScopeTo<TExodata, TContextBase, TSubjectBase> Bind<TExodata>(IExodataDeclaration<TExodata> declaration);
        IFluentExodataBindingSubjectGivenWhenScopeTo<TExodata, TContextBase, TSubjectBase> Bind<TExodata>(IExodataDeclaration declaration);

        void Bind<TExodata>(IExodataDeclaration<TExodata> declaration, TExodata value);
        void Bind<TExodata>(IExodataDeclaration declaration, TExodata value);
    }

    public interface IFluentExodataBindingSubjectGivenWhenScopeTo<TExodata, TContextBase, TSubjectBase> : IFluentExodataBindingGivenWhenScopeTo<TExodata, TContextBase, TSubjectBase>
    {
        IFluentExodataBindingGivenWhenScopeTo<TExodata, TContextBase, TSubjectBase> For(TSubjectBase subject);
        IFluentExodataBindingGivenWhenScopeTo<TExodata, TContextBase, TSubjectBase> For(Expression<Func<TSubjectBase, object>> member);
        IFluentExodataBindingGivenWhenScopeTo<TExodata, TContextBase, TSubjectBase> For(TSubjectBase subject, Expression<Func<TSubjectBase, object>> member);

        IFluentExodataBindingGivenWhenScopeTo<TExodata, TContextBase, TSubject> For<TSubject>() where TSubject : TSubjectBase;
        IFluentExodataBindingGivenWhenScopeTo<TExodata, TContextBase, TSubject> For<TSubject>(TSubject subject) where TSubject : TSubjectBase;
        IFluentExodataBindingGivenWhenScopeTo<TExodata, TContextBase, TSubject> For<TSubject>(Expression<Func<TSubject, object>> member) where TSubject : TSubjectBase;
        IFluentExodataBindingGivenWhenScopeTo<TExodata, TContextBase, TSubject> For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member) where TSubject : TSubjectBase;
    }

    public interface IFluentExodataBindingGivenWhenScopeTo<TExodata, TContextBase, TSubject> : IFluentExodataBindingWhenScopeTo<TExodata, TContextBase, TSubject>
    {
        IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubject> Given<TContext>(Maybe<TContext> context = default(Maybe<TContext>)) where TContext : TContextBase;
    }

    public interface IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubject> : IFluentExodataBindingScopeTo<TExodata, TContext, TSubject>
    {
        IFluentExodataBindingScopeTo<TExodata, TContext, TSubject> When(Func<IExodataRequest<TContext, TSubject>, bool> predicate);
    }

    public interface IFluentExodataBindingScopeTo<TExodata, TContext, TSubject> : IFluentExodataBindingTo<TExodata, TContext, TSubject>
    {
        IFluentExodataBindingTo<TExodata, TContext, TSubject> InScope(object scopeObject);
        IFluentExodataBindingTo<TExodata, TContext, TSubject> InScope(Func<IExodataRequest<TContext, TSubject>, object> scopeFactory);
    }

    public interface IFluentExodataBindingTo<TExodata, TContext, TSubject> : IFluentInterface
    {
        void To(TExodata value);
        void To(Func<IExodataRequest<TContext, TSubject>, TExodata> valueFactory);
    }
}
