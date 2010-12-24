using System;
using System.Reflection.Emit;

namespace iSynaptic.Commons.Data
{
    public class MaxLengthAttribute : Attribute, IMetadataAttribute
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

        public TMetadata Resolve<TMetadata>(MetadataRequest<TMetadata> request)
        {
            return Cast.To<int, TMetadata>(_MaxLength);
        }
    }
}