using System;

namespace iSynaptic.Commons.Data
{
    public class MaxLengthAttribute : Attribute, IMetadataAttribute<int>
    {
        private readonly int _MaxLength;

        public MaxLengthAttribute(int maxLength)
        {
            _MaxLength = maxLength;
        }

        public bool ProvidesMetadataFor(MetadataRequest<int> request)
        {
            return request.Declaration == StringMetadata.MaxLength;
        }

        public int Resolve(MetadataRequest<int> request)
        {
            return _MaxLength;
        }
    }
}