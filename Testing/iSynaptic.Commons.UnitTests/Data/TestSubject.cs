namespace iSynaptic.Commons.Data
{
    public class TestSubject
    {
        [StringMetadata(21, 84, "First Name")]
        public string FirstName { get; set; }

        [StringMetadata(7, 1764, "Last Name")]
        public string LastName = null;

        public string MiddleName { get; set; }
    }
}