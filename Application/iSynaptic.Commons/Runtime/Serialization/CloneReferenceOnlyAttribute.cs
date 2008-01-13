using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Runtime.Serialization
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class CloneReferenceOnlyAttribute : Attribute
    {
        public CloneReferenceOnlyAttribute()
        {
        }
    }
}
