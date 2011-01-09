using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public static class Cast<TSource, TDestination>
    {
        static Cast()
        {
            if(typeof(TDestination).IsAssignableFrom(typeof(TSource)) != true)
                throw new InvalidOperationException(string.Format("Object of type {0} cannot be casted to {1}.", typeof(TSource).Name, typeof(TDestination).Name));
        }

        public static TDestination With(TSource source)
        {
            throw new NotImplementedException();
        }
    }
}
