namespace iSynaptic.Commons.Collections.Generic
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
