using iSynaptic.Commons.Data.ExodataDeclarations;

namespace iSynaptic.Commons.Data
{
    public static class StringExodata
    {
        public readonly static ExodataDeclaration<StringExodataDefinition> All = new ExodataDeclaration<StringExodataDefinition>();

        public readonly static ComparableExodataDeclaration<int> MinLength = new ComparableExodataDeclaration<int>(0, int.MaxValue, 0);
        public readonly static ComparableExodataDeclaration<int> MaxLength = new ComparableExodataDeclaration<int>(0, int.MaxValue, int.MaxValue);
    }

    public class StringExodataDefinition : CommonExodataDefinition
    {
        public StringExodataDefinition(int minLength, int maxLength, string description) : base(description)
        {
            MinimumLength = minLength;
            MaximumLength = maxLength;
        }

        public int MinimumLength { get; private set; }
        public int MaximumLength { get; private set; }
    }
}