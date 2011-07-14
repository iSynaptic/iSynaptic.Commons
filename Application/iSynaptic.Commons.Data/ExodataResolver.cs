using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using iSynaptic.Commons;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Data
{
    public class ExodataResolver : IExodataResolver
    {
        private readonly HashSet<IExodataBindingSource> _BindingSources = new HashSet<IExodataBindingSource>();

        public Maybe<TExodata> TryResolve<TExodata, TContext, TSubject>(ISymbol<TExodata> symbol, Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            Guard.NotNull(symbol, "symbol");

            var request = ExodataRequest.Create(symbol, context, subject, member);

            var candidateBindings = _BindingSources
                .SelectMany(x => x.GetBindingsFor(request))
                .Where(x => x.Matches(request));

            var selectedBinding = SelectBinding(request, candidateBindings);

            if(selectedBinding == null)
                return Maybe<TExodata>.NoValue;

            return selectedBinding.Resolve(request);
        }

        protected virtual IExodataBinding SelectBinding<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request, IEnumerable<IExodataBinding> candidates)
        {
            Guard.NotNull(candidates, "candidates");

            try
            {
                return candidates.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("More than one Exodata binding was found. Remove duplicate bindings or apply additional conditions to existing bindings to make them unambiguous.", ex);
            }
        }

        public T AddExodataBindingSource<T>() where T : IExodataBindingSource, new()
        {
            T source = new T();
            AddExodataBindingSource(source);

            return source;
        }

        public void AddExodataBindingSource(IExodataBindingSource source)
        {
            Guard.NotNull(source, "source");

            _BindingSources.Add(source);
        }

        public void RemoveExodataBindingSource(IExodataBindingSource source)
        {
            Guard.NotNull(source, "source");
            _BindingSources.Remove(source);
        }
    }
}
