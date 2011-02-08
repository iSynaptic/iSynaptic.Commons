using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public static class DependencyResolverExtensions
    {
        private static Func<IDependencyResolver, Type, string, object> _ResolveStrategy = null;

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
            return ResolveStrategy(resolver, dependencyType, name);
        }

        public static void SetResolveStrategy(Func<IDependencyResolver, Type, string, object> strategy)
        {
            _ResolveStrategy = strategy;
        }

        private static Func<IDependencyResolver, Type, string, object> ResolveStrategy
        {
            get { return _ResolveStrategy ?? DefaultResolveStrategy; }
        }

        private static object DefaultResolveStrategy(IDependencyResolver resolver, Type dependencyType, string name)
        {
            Guard.NotNull(resolver, "resolver");
            Guard.NotNull(dependencyType, "dependencyType");

            return resolver.Resolve(new DepencencyDeclaration(dependencyType, name));
        }
    }
}
