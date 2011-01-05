using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class SurrogateMetadataBindingSourceAttribute : Attribute
    {
        public SurrogateMetadataBindingSourceAttribute(Type realType)
        {
            Guard.NotNull(realType, "realType");

            RealType = realType;
        }

        public Type RealType { get; private set; }
    }
}
