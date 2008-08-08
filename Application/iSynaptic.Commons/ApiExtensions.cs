using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace iSynaptic.Commons
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ApiExtensions
    {
        public static T Resolve<T>(this IDependencyResolver self)
        {
            return Resolve<T>(self, null);
        }

        public static T Resolve<T>(this IDependencyResolver self, object context)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            return (T)self.Resolve(typeof(T), context);
        }
    }
}
