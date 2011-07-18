using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iSynaptic.Commons.Collections.Generic;
using iSynaptic.Commons.Linq;

namespace iSynaptic.Commons.Data
{
    public class SurrogateExodataBindingSource : IExodataBindingSource
    {
        private readonly Lazy<KeyValuePair<Type, IExodataBindingSource>[]> _Surrogates = new Lazy<KeyValuePair<Type, IExodataBindingSource>[]>(() =>
        {
            Type bindingSourceType = typeof(IExodataBindingSource);

            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.IsDynamic != true)
                .SelectMany(x => x.GetExportedTypes())
                .Where(x => !x.IsAbstract && !x.IsGenericTypeDefinition && bindingSourceType.IsAssignableFrom(x))
                .Select(x => new { Type = x, BaseType = GetExodataSurrgateBaseClass(x) })
                .Where(x => x.BaseType.HasValue)
                .Select(x => KeyValuePair.Create(x.BaseType.Value.GetGenericArguments()[0], InstantiateSurrogate(x.Type)))
                .ToArray();
        });

        private static Maybe<Type> GetExodataSurrgateBaseClass(Type type)
        {
            return type.Recurse(x => Maybe.NotNull(x.BaseType))
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof (ExodataSurrogate<>))
                .FirstOrDefault()
                .ToMaybe()
                .NotNull();
        }

        private static IExodataBindingSource InstantiateSurrogate(Type type)
        {
            object surrogate = Ioc.Resolve(type);
            
            if(surrogate == null && type.GetConstructors().Any(x => x.GetParameters().Length == 0))
                surrogate = Activator.CreateInstance(type);

            if (surrogate == null)
                throw new InvalidOperationException(string.Format("Unable to instantiate Exodata surrogate '{0}'. The surrogate is not registered with Ioc and has no public parameterless constructor.", type.FullName));

            return (IExodataBindingSource)surrogate;
        }

        public IEnumerable<IExodataBinding> GetBindingsFor<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            Type subjectType = typeof(TSubject);

            return _Surrogates.Value
                .Where(x => x.Key.IsAssignableFrom(subjectType))
                .Select(x => x.Value)
                .SelectMany(x => x.GetBindingsFor(request));
        }
    }
}
