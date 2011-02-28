using System;
using System.Collections.Generic;

namespace iSynaptic.Commons.Data
{
    public class TestSubjectExodataSurrogate : ExodataSurrogate<TestSubject>
    {
        public static readonly TestSubject Subject = new TestSubject();

        public TestSubjectExodataSurrogate()
        {
            ScopeObject = new object();

            Bind(StringExodata.MaxLength)
                .For(x => x.MiddleName)
                .To(74088);

            Bind(CommonExodata.Description)
                .When(r => ShouldYieldInstanceExodata)
                .To("Surrogate Description");

            Bind(CommonExodata.Description)
                .For(Subject)
                .When(r => ShouldYieldInstanceExodata)
                .InScope(ScopeObject)
                .To(r => "Special Instance Description");

            Bind(CommonExodata.Description)
                .For(Subject, x => x.FirstName)
                .When(r => ShouldYieldInstanceExodata)
                .InScope(r => ScopeObject)
                .To("Special Member Description");
        }

        public static bool ShouldYieldInstanceExodata { get; set; }
        public static object ScopeObject { get; set; }
    }
}