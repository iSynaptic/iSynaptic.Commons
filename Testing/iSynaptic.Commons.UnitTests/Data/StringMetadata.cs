namespace iSynaptic.Commons.Data
{
    public class StringMetadata
    {
        public readonly static IntegerMetadataDeclaration MaxLength = new IntegerMetadataDeclaration(0, int.MaxValue, int.MaxValue);
        public readonly static IntegerMetadataDeclaration MinLength = new IntegerMetadataDeclaration(0, int.MaxValue, 0);
    }
}