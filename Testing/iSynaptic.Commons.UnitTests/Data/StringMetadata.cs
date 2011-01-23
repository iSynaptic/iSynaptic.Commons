using iSynaptic.Commons.Data.MetadataDeclarations;

namespace iSynaptic.Commons.Data
{
    public static class StringMetadata
    {
        public readonly static MetadataDeclaration<StringMetadataDefinition> All = new MetadataDeclaration<StringMetadataDefinition>();

        public readonly static ComparableMetadataDeclaration<int> MinLength = new ComparableMetadataDeclaration<int>(0, int.MaxValue, 0);
        public readonly static ComparableMetadataDeclaration<int> MaxLength = new ComparableMetadataDeclaration<int>(0, int.MaxValue, int.MaxValue);
    }

    public class StringMetadataDefinition : CommonMetadataDefinition
    {
        public StringMetadataDefinition(int minLength, int maxLength, string description) : base(description)
        {
            MinimumLength = minLength;
            MaximumLength = maxLength;
        }

        public int MinimumLength { get; private set; }
        public int MaximumLength { get; private set; }
    }
}