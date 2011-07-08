namespace iSynaptic.Commons.Data
{
    public class TestExodataBindingModule : ExodataBindingModule
    {
        public TestExodataBindingModule()
        {
            Bind(StringExodata.MaxLength, 42);

            Bind(CommonExodata.Description)
                .For<TestSubject>()
                .To("Overridden Description");
        }
    }
}