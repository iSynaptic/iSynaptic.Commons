using System;
using System.Collections.Generic;

namespace iSynaptic.Commons.Data
{
    public class TestSubjectExodataSurrogate : ExodataSurrogate<TestSubject>
    {
        public static readonly TestSubject Subject = new TestSubject();

        public TestSubjectExodataSurrogate()
        {
            Bind(StringExodata.MaxLength)
                .For(x => x.MiddleName)
                .To(74088);

            Bind(CommonExodata.Description)
                .When(r => ShouldYieldInstanceExodata)
                .To("Surrogate Description");

            Bind(CommonExodata.Description)
                .For(Subject)
                .When(r => ShouldYieldInstanceExodata)
                .To(r => "Special Instance Description");

            Bind(CommonExodata.Description)
                .For(Subject, x => x.FirstName)
                .When(r => ShouldYieldInstanceExodata)
                .To("Special Member Description");

            Bind(CommonExodata.Description)
                .Given<string>()
                .For(x => x.FirstName)
                .When(r => ShouldYieldInstanceExodata)
                .To("Contextual Member Description");

            Bind(CommonExodata.Description)
                .Given("Context")
                .For(x => x.FirstName)
                .When(r => ShouldYieldInstanceExodata)
                .To("Specific Contextual Member Description");
        }

        public static bool ShouldYieldInstanceExodata { get; set; }
    }
}