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
        private readonly IExodataBindingSource[] _BindingSources = null;

        public ExodataResolver(IEnumerable<IExodataBindingSource> bindingSources)
        {
            _BindingSources = Guard.NotNullOrEmpty(bindingSources, "bindingSources")
                .ToArray();
        }

        protected ExodataResolver()
        {
            _BindingSources = new IExodataBindingSource[0];
        }

        public Maybe<TExodata> TryResolve<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            Guard.NotNull(request, "request");
            Guard.MustSatisfy(request, r => r.Symbol != null, "request", "The request's symbol cannot be null.");

            var candidateBindings = GetBindingSources()
                .SelectMany(x => x.GetBindingsFor(request))
                .Select(x => new { Binding = x, Result = x.TryResolve(request) })
                .TakeWhile(x => x.Result.Exception == null)
                .Where(x => x.Result.HasValue || x.Result.Exception != null)
                .ToList();

            if (candidateBindings.Count > 0)
            {
                var lastBinding = candidateBindings[candidateBindings.Count - 1];

                if (lastBinding.Result.Exception != null)
                    return lastBinding.Result;

                candidateBindings.Sort((l, r) => CompareBindingPrecidence(request, l.Binding, r.Binding));

                var finalBindings = candidateBindings
                    .TakeWhile(
                        (x, i) =>
                        i == 0 || CompareBindingPrecidence(request, candidateBindings[i - 1].Binding, x.Binding) == 0)
                    .ToArray();

                if (finalBindings.Length > 1)
                    return Maybe.Exception<TExodata>(new AmbiguousExodataBindingsException("More than one Exodata binding was found. Remove duplicate bindings or apply additional conditions to existing bindings to make them unambiguous.", finalBindings.Select(x => x.Binding)));

                if (finalBindings.Length == 1)
                    return finalBindings[0].Result;
            }

            return Maybe<TExodata>.NoValue;
        }

        protected virtual int CompareBindingPrecidence<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request, IExodataBinding left, IExodataBinding right)
        {
            return 0;
        }

        public virtual IEnumerable<IExodataBindingSource> GetBindingSources()
        {
            return _BindingSources;
        }
    }
}
