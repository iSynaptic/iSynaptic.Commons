using System;

namespace iSynaptic.Commons.Data
{
    public class IntegerMetadataDeclaration : MetadataDeclaration<int>
    {
        public int Max { get; private set; }
        public int Min { get; private set; }

        public IntegerMetadataDeclaration(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public IntegerMetadataDeclaration(int min, int max, int @default)
            : base(@default)
        {
            Min = min;
            Max = max;
        }

        protected override void OnCheckValue(int value, string valueName)
        {
            base.OnCheckValue(value, valueName);

            if (Min > value || value > Max)
                throw new ArgumentOutOfRangeException("value");
        }
    }
}