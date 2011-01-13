using iSynaptic.Commons.Data.MetadataDeclarations;

namespace iSynaptic.Commons.Data
{
    public class StringMetadata
    {
        public readonly static MetadataDeclaration<StringMetadata> All = new MetadataDeclaration<StringMetadata>();

        public readonly static ComparableMetadataDeclaration<int> MinLength = new ComparableMetadataDeclaration<int>(0, int.MaxValue, 0);
        public readonly static ComparableMetadataDeclaration<int> MaxLength = new ComparableMetadataDeclaration<int>(0, int.MaxValue, int.MaxValue);

        public StringMetadata(int minLength, int maxLength, string description)
        {
            MinimumLength = minLength;
            MaximumLength = maxLength;
            Description = description;
        }

        public int MinimumLength { get; private set; }
        public int MaximumLength { get; private set; }
        public string Description { get; private set; }
    }
}