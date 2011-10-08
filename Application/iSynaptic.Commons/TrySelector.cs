using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public delegate bool TrySelector<in T, TResult>(T input, out TResult result);
}
