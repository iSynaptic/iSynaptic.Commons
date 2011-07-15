namespace iSynaptic.Commons.Data
{
    public class IntegerExodata
    {
        public readonly static ComparableExodataDeclaration<int> MinValue = new ComparableExodataDeclaration<int>(0, int.MaxValue, 0);
        public readonly static ComparableExodataDeclaration<int> MaxValue = new ComparableExodataDeclaration<int>(0, int.MaxValue, int.MaxValue);
    }
}