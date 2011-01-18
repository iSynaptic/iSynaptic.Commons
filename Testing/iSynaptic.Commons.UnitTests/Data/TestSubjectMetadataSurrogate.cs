using System;
using System.Collections.Generic;

namespace iSynaptic.Commons.Data
{
    public class TestSubjectMetadataSurrogate : MetadataSurrogate<TestSubject>
    {
        public static readonly TestSubject Subject = new TestSubject();

        public TestSubjectMetadataSurrogate()
        {
            ScopeObject = new object();

            Bind(StringMetadata.MaxLength, x => x.MiddleName)
                .To(74088);

            Bind(CommonMetadata.Description)
                .When(r => ShouldYieldInstanceMetadata)
                .To("Surrogate Description");

            Bind(CommonMetadata.Description, Subject)
                .When(r => ShouldYieldInstanceMetadata)
                .InScope(ScopeObject)
                .To(r => "Special Instance Description");

            Bind(CommonMetadata.Description, Subject, x => x.FirstName)
                .When(r => ShouldYieldInstanceMetadata)
                .InScope(r => ScopeObject)
                .To("Special Member Description");
        }

        public static bool ShouldYieldInstanceMetadata { get; set; }
        public static object ScopeObject { get; set; }
    }
}