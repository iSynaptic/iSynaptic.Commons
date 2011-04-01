using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public class ExodataBindingModule : IExodataBindingSource, IFluentExodataBindingRoot<object, object>, IFluentInterface
    {
        private readonly HashSet<IExodataBinding> _Bindings = new HashSet<IExodataBinding>();

        public IEnumerable<IExodataBinding> GetBindingsFor<TExodata, TContext, TSubject>(IExodataRequest<TContext, TSubject> request)
        {
            return _Bindings;
        }

        public IFluentExodataBindingGivenSubjectWhenScopeTo<TExodata, object, object> Bind<TExodata>(IExodataDeclaration<TExodata> declaration)
        {
            return Bind<TExodata>((IExodataDeclaration) declaration);
        }

        public IFluentExodataBindingGivenSubjectWhenScopeTo<TExodata, object, object> Bind<TExodata>(IExodataDeclaration declaration)
        {
            Guard.NotNull(declaration, "declaration");
            return new FluentExodataBindingBuilder<TExodata, object, object>(this, declaration, b => _Bindings.Add(b));
        }

        public void Bind<TExodata>(IExodataDeclaration<TExodata> declaration, TExodata value)
        {
            Bind(declaration).To(value);
        }

        public void Bind<TExodata>(IExodataDeclaration declaration, TExodata value)
        {
            Bind<TExodata>(declaration).To(value);
        }
    }
}
