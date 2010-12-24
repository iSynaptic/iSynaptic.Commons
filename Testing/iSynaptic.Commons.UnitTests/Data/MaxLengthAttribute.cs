using System;
using System.Reflection.Emit;

namespace iSynaptic.Commons.Data
{
    public class MaxLengthAttribute : Attribute, IMetadataAttribute<int>
    {
        private readonly int _MaxLength;

        public MaxLengthAttribute(int maxLength)
        {
            _MaxLength = maxLength;
        }

        public bool ProvidesMetadataFor<TMetadata>(MetadataRequest<TMetadata> request)
        {
            return request.Declaration == StringMetadata.MaxLength;
        }

        public int Resolve(MetadataRequest<int> request)
        {
            return _MaxLength;
        }
    }
}