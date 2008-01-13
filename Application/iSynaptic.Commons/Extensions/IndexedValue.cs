using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Extensions
{
    public struct IndexedValue<T>
    {
        public IndexedValue(int index, T value)
        {
            Index = index;
            Value = value;
        }

        public readonly int Index;
        public readonly T Value;
    }
}
