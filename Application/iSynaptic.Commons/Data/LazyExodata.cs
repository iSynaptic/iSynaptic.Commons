using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class LazyExodata<TExodata> : Lazy<TExodata>
    {
        public LazyExodata(Func<TExodata> resolutionStrategy)
            : base(resolutionStrategy)
        {
            Guard.NotNull(resolutionStrategy, "resolutionStrategy");
        }

        public static implicit operator TExodata(LazyExodata<TExodata> value)
        {
            return value.Value;
        }
    }
}
