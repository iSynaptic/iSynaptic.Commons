using System;
using System.Collections.Generic;

namespace iSynaptic.Commons.Data
{
    [SurrogateMetadataBindingSource(typeof(TestSubject))]
    public class TestSubjectSurrogateMetadataBindingSource : IMetadataBindingSource
    {
        public IEnumerable<IMetadataBinding<TMetadata>> GetBindingsFor<TMetadata>(MetadataRequest<TMetadata> request)
        {
            if(request.Declaration != StringMetadata.MaxLength)
                yield break;

            if((Type)request.Subject != typeof(TestSubject))
                yield break;

            if(request.Member.Name != "MiddleName")
                yield break;

            yield return new MetadataBinding<TMetadata>(request.Declaration, 74088);
        }
    }
}