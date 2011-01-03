using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class SurrogateMetadataBindingSourceAttribute : Attribute
    {
        public SurrogateMetadataBindingSourceAttribute(Type realType)
        {
            if (realType == null)
                throw new ArgumentNullException("realType");

            RealType = realType;
        }

        public Type RealType { get; private set; }
    }
}
