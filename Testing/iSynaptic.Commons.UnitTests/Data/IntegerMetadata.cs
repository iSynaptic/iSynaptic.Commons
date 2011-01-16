using iSynaptic.Commons.Data.MetadataDeclarations;

namespace iSynaptic.Commons.Data
{
    public class IntegerMetadata
    {
        public readonly static ComparableMetadataDeclaration<int> MinValue = new ComparableMetadataDeclaration<int>(0, int.MaxValue, 0);
        public readonly static ComparableMetadataDeclaration<int> MaxValue = new ComparableMetadataDeclaration<int>(0, int.MaxValue, int.MaxValue);
    }
}