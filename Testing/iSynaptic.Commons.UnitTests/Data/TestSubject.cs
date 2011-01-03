namespace iSynaptic.Commons.Data
{
    public class TestSubject
    {
        [MaxLength(84)]
        public string FirstName { get; set; }

        [MaxLength(1764)]
        public string LastName = null;

        public string MiddleName { get; set; }
    }
}