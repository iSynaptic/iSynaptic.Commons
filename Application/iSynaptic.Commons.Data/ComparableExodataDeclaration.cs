using System;

namespace iSynaptic.Commons.Data
{
    public class ComparableExodataDeclaration<T> : ExodataDeclaration<T> where T : IComparable<T>
    {
        public ComparableExodataDeclaration(T minValue, T maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public ComparableExodataDeclaration(T minValue, T maxValue, T @default)
            : base(@default)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        protected override Maybe<T> EnsureValid(T value, string valueName)
        {
            if(value.CompareTo(MinValue) < 0)
                return Maybe.Exception<T>(new ExodataValidationException<T>(this, value, string.Format("The {0} value must be greater than or equal to {1}.", valueName, MinValue)));

            if(value.CompareTo(MaxValue) > 0)
                return Maybe.Exception<T>(new ExodataValidationException<T>(this, value, string.Format("The {0} value must be less than or equal to {1}.", valueName, MaxValue)));

            return base.EnsureValid(value, valueName);
        }

        public T MinValue { get; private set; }
        public T MaxValue { get; private set; }
    }
}
