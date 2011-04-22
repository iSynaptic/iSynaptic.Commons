using System;
using System.Diagnostics;

namespace iSynaptic.Commons
{
    public static class Ioc
    {
        private class NullDependencyResolver : IDependencyResolver
        {
            public Maybe<object> TryResolve(IDependencyDeclaration declaration)
            {
                return Maybe<object>.NoValue;
            }
        }

        private static IDependencyResolver _DependencyResolver = null;

        public static T Resolve<T>()
        {
            return DependencyResolver.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return DependencyResolver.Resolve<T>(name);
        }

        public static object Resolve(Type dependencyType)
        {
            return DependencyResolver.Resolve(dependencyType);
        }

        public static object Resolve(Type dependencyType, string name)
        {
            return DependencyResolver.Resolve(dependencyType, name);
        }

        public static Maybe<T> TryResolve<T>()
        {
            return DependencyResolver.TryResolve<T>();
        }

        public static Maybe<T> TryResolve<T>(string name)
        {
            return DependencyResolver.TryResolve<T>(name);
        }

        public static Maybe<object> TryResolve(Type dependencyType)
        {
            return DependencyResolver.TryResolve(dependencyType);
        }

        public static Maybe<object> TryResolve(Type dependencyType, string name)
        {
            return DependencyResolver.TryResolve(dependencyType, name);
        }

        public static void SetDependencyResolver(IDependencyResolver resolver)
        {
            DependencyResolver = resolver;
        }

        private static IDependencyResolver DependencyResolver
        {
            get { return _DependencyResolver ?? new NullDependencyResolver(); }
            set { _DependencyResolver = value; }
        }
    }
}
