using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Runtime.Serialization
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class CloneReferenceOnlyAttribute : Attribute
    {
    }
}
