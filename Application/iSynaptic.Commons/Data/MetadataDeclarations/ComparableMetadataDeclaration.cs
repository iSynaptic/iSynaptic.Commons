using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data.MetadataDeclarations
{
    public class ComparableMetadataDeclaration<T> : MetadataDeclaration<T> where T : IComparable<T>
    {
        public ComparableMetadataDeclaration(T minValue, T maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public ComparableMetadataDeclaration(T minValue, T maxValue, T @default)
            : base(@default)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        protected override void OnValidateValue(T value, string valueName)
        {
            if(value.CompareTo(MinValue) < 0)
                throw new MetadataValidationException<T>(this, value, string.Format("The {0} value must be greater than or equal to {1}.", valueName, MinValue));

            if(value.CompareTo(MaxValue) > 0)
                throw new MetadataValidationException<T>(this, value, string.Format("The {0} value must be less than or equal to {1}.", valueName, MaxValue));

            base.OnValidateValue(value, valueName);
        }

        public T MinValue { get; private set; }
        public T MaxValue { get; private set; }
    }
}
