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

        public IEnumerable<IExodataBinding> GetBindingsFor<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            return _Bindings;
        }

        public IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, object, object> Bind<TExodata>(ISymbol<TExodata> symbol)
        {
            Guard.NotNull(symbol, "symbol");
            return Bind<TExodata>((ISymbol)symbol);
        }

        public IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, object, object> Bind<TExodata>(ISymbol symbol)
        {
            Guard.NotNull(symbol, "symbol");
            return new FluentExodataBindingBuilder<TExodata, object, object>(this, symbol.ToMaybe(), b => _Bindings.Add(b));
        }

        public void Bind<TExodata>(ISymbol<TExodata> symbol, TExodata value, string name = null)
        {
            Bind<TExodata>((ISymbol)symbol).Named(name).To(value);
        }

        public void Bind<TExodata>(ISymbol symbol, TExodata value, string name = null)
        {
            Bind<TExodata>(symbol).Named(name).To(value);
        }
    }
}
