using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace iSynaptic.Commons
{
    public static class IoC
    {
        private static IDependencyResolver _DependencyResolver = null;

        public static T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public static T Resolve<T>(string key)
        {
            return (T)Resolve(typeof(T), key);
        }

        public static T Resolve<T>(string key, Type requestingType)
        {
            return (T)Resolve(typeof(T), key, requestingType);
        }

        public static object Resolve(Type dependencyType)
        {
            return Resolve(dependencyType, null);
        }

        public static object Resolve(Type dependencyType, string key)
        {
            Type requestingType = typeof(IoC);

            var stackTrace = new StackTrace(1);

            int currentFrame = 0;
            while(requestingType == typeof(IoC))
            {
                var stackFrame = stackTrace.GetFrame(currentFrame++);
                var method = stackFrame.GetMethod();

                if(method.DeclaringType != typeof(IoC))
                    requestingType = method.DeclaringType;
            }

            return Resolve(dependencyType, key, requestingType);
        }

        public static object Resolve(Type dependencyType, string key, Type requestingType)
        {
            if(_DependencyResolver == null)
                return null;

            return _DependencyResolver.Resolve(key, dependencyType, requestingType);
        }

        public static void SetDependencyResolver(IDependencyResolver dependencyResolver)
        {
            _DependencyResolver = dependencyResolver;
        }
    }
}
