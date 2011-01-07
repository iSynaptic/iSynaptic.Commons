using System;
using System.Collections.Generic;

namespace iSynaptic.Commons.Data
{
    [SurrogateMetadataBindingSource(typeof(TestSubject))]
    public class TestSubjectSurrogateMetadataBindingSource : MetadataBindingModule
    {
        public TestSubjectSurrogateMetadataBindingSource()
        {
            Bind(StringMetadata.MaxLength)
                .For<TestSubject>(x => x.MiddleName)
                .To(74088);
        }
    }
}