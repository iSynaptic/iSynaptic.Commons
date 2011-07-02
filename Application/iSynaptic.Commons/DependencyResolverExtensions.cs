using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public static class DependencyResolverExtensions
    {
        private static Func<IDependencyResolver, Type, string, Maybe<object>> _ResolveStrategy = null;

        public static T Resolve<T>(this IDependencyResolver resolver)
        {
            return (T)Resolve(resolver, typeof(T));
        }

        public static T Resolve<T>(this IDependencyResolver resolver, string name)
        {
            return (T)Resolve(resolver, typeof(T), name);
        }

        public static object Resolve(this IDependencyResolver resolver, Type dependencyType)
        {
            return Resolve(resolver, dependencyType, null);
        }

        public static object Resolve(this IDependencyResolver resolver, Type dependencyType, string name)
        {
            return ResolveStrategy(resolver, dependencyType, name).ValueOrDefault();
        }

        public static Maybe<T> TryResolve<T>(this IDependencyResolver resolver)
        {
            return TryResolve(resolver, typeof(T)).Cast<T>();
        }

        public static Maybe<T> TryResolve<T>(this IDependencyResolver resolver, string name)
        {
            return TryResolve(resolver, typeof(T), name).Cast<T>();
        }

        public static Maybe<object> TryResolve(this IDependencyResolver resolver, Type dependencyType)
        {
            return TryResolve(resolver, dependencyType, null);
        }

        public static Maybe<object> TryResolve(this IDependencyResolver resolver, Type dependencyType, string name)
        {
            return ResolveStrategy(resolver, dependencyType, name);
        }

        public static void SetResolveStrategy(Func<IDependencyResolver, Type, string, Maybe<object>> strategy)
        {
            _ResolveStrategy = strategy;
        }

        private static Func<IDependencyResolver, Type, string, Maybe<object>> ResolveStrategy
        {
            get { return _ResolveStrategy ?? DefaultResolveStrategy; }
        }

        private static Maybe<object> DefaultResolveStrategy(IDependencyResolver resolver, Type dependencyType, string name)
        {
            Guard.NotNull(resolver, "resolver");
            Guard.NotNull(dependencyType, "dependencyType");

            return resolver.TryResolve(new DepencencyDeclaration(dependencyType, name));
        }
    }
}
