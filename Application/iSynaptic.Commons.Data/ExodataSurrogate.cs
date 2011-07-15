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

        IEnumerable<IExodataBinding> IExodataBindingSource.GetBindingsFor<TExodata, TContext, TRequestSubject>(IExodataRequest<TExodata, TContext, TRequestSubject> request)
        {
            return _Bindings;
        }

        public IFluentExodataBindingGivenSubjectWhenTo<TExodata, object, TSubject> Bind<TExodata>(ISymbol<TExodata> symbol)
        {
            Guard.NotNull(symbol, "symbol");
            return new FluentExodataBindingBuilder<TExodata, object, TSubject>(this, symbol, b => _Bindings.Add(b));
        }

        public void Bind<TExodata>(ISymbol<TExodata> symbol, TExodata value)
        {
            Bind(symbol).To(value);
        }
    }
}
