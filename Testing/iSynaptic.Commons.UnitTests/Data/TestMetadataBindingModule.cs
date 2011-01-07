namespace iSynaptic.Commons.Data
{
    public class TestMetadataBindingModule : MetadataBindingModule
    {
        public TestMetadataBindingModule()
        {
            Bind(StringMetadata.MaxLength, 42);

            Bind(CommonMetadata.Description)
                .For<TestSubject>()
                .To("Overriden Description");
        }
    }
}