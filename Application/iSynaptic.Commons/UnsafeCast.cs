using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Stubs
{
    public static class UnsafeCast<TSource, TDestination>
    {
        public static TDestination With(TSource source)
        {
            return IL.UnsafeCast<TSource, TDestination>.With(source);
        }
    }
}
