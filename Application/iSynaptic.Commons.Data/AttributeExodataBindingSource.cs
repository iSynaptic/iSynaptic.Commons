using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iSynaptic.Commons.Collections.Generic;
using iSynaptic.Commons.Reflection;

namespace iSynaptic.Commons.Data
{
    public class AttributeExodataBindingSource : IExodataBindingSource
    {
        private readonly LazySelectionDictionary<ICustomAttributeProvider, IEnumerable<IExodataBinding>> _Bindings =
            new LazySelectionDictionary<ICustomAttributeProvider, IEnumerable<IExodataBinding>>(x => GetBindings(x).ToMaybe());

        public IEnumerable<IExodataBinding> GetBindingsFor<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            Guard.NotNull(request, "request");

            ICustomAttributeProvider provider = request.Member ?? 
                request.Subject
                .Where(x => x != null)
                .Select(x => x.GetType())
                .ValueOrDefault(typeof(TSubject));

            return _Bindings[provider]
                .Select(x => ExodataBinding.Create<TExodata, object, TSubject>(null, this, Maybe<ISymbol>.NoValue, Maybe<object>.NoValue, request.Subject, request.Member != null ? new[]{request.Member} : null, null, r => x.TryResolve(r).AsMaybe()));
        }

        private static IEnumerable<IExodataBinding> GetBindings(ICustomAttributeProvider provider)
        {
            return provider.GetCustomAttributes(true)
                .Select(x => new
                {
                    Attribute = x,
                    Interfaces = x.GetType().GetInterfaces().Where(y => y.DoesImplementType(typeof(IExodataAttribute<>))).ToArray()
                })
                .Where(x => x.Interfaces.Length > 0)
                .SelectMany(x => x.Interfaces.Select(y => WrapAttribute(x.Attribute, y.GetGenericArguments()[0])));
        }

        private static IExodataBinding WrapAttribute(object attribute, Type exodataType)
        {
            return (IExodataBinding)Activator.CreateInstance(typeof(AttributeBinding<>).MakeGenericType(exodataType), attribute);
        }

        private class AttributeBinding<TExodata> : IExodataBinding
        {
            private readonly IExodataAttribute<TExodata> _Attribute;

            public AttributeBinding(IExodataAttribute<TExodata> attribute)
            {
                _Attribute = attribute;
            }

            public Maybe<TRequestExodata> TryResolve<TRequestExodata, TContext, TSubject>(IExodataRequest<TRequestExodata, TContext, TSubject> request)
            {
                return ExodataRequest<TExodata, TContext, TSubject>
                    .TryToAdapt(request)
                    .SelectMaybe(r => _Attribute.TryResolve(r))
                    .Cast<TRequestExodata>();
            }
        }
    }
}
