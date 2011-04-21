using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public abstract class ExodataSurrogate<TSubject> : IExodataBindingSource, IFluentExodataBindingRoot<object, TSubject>, IFluentInterface
    {
        private readonly HashSet<IExodataBinding> _Bindings = new HashSet<IExodataBinding>();

        IEnumerable<IExodataBinding> IExodataBindingSource.GetBindingsFor<TExodata, TContext, TBindingSubject>(IExodataRequest<TExodata, TContext, TBindingSubject> request)
        {
            return _Bindings;
        }

        public IFluentExodataBindingGivenSubjectWhenScopeTo<TExodata, object, TSubject> Bind<TExodata>(IExodataDeclaration<TExodata> declaration)
        {
            Guard.NotNull(declaration, "declaration");
            return new FluentExodataBindingBuilder<TExodata, object, TSubject>(this, declaration, b => _Bindings.Add(b));
        }

        public void Bind<TExodata>(IExodataDeclaration<TExodata> declaration, TExodata value)
        {
            Bind(declaration).To(value);
        }
    }
}
