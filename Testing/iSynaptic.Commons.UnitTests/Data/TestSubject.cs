namespace iSynaptic.Commons.Data
{
    [Description("Test Subject")]
    public class TestSubject
    {
        [StringExodata(21, 84, "First Name")]
        public string FirstName { get; set; }

        [StringExodata(7, 1764, "Last Name")]
        public string LastName = null;

        public string MiddleName { get; set; }
    }

    [Description("Derived Test Subject")]
    public class DerivedTestSubject : TestSubject {}
}