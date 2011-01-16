using System;
using System.Reflection.Emit;

namespace iSynaptic.Commons.Data
{
    public class StringMetadataAttribute : Attribute, IMetadataAttribute<int>, IMetadataAttribute<string>, IMetadataAttribute<StringMetadata>
    {
        private readonly int _MinLength;
        private readonly int _MaxLength;
        private readonly string _Description;

        public StringMetadataAttribute(int minLength, int maxLength, string description)
        {
            _MinLength = minLength;
            _MaxLength = maxLength;
            _Description = description;
        }

        public bool ProvidesMetadataFor<TMetadata>(MetadataRequest<TMetadata> request)
        {
            var declaration = request.Declaration;

            return
                ReferenceEquals(declaration, StringMetadata.All) ||
                ReferenceEquals(declaration, StringMetadata.MinLength) ||
                ReferenceEquals(declaration, StringMetadata.MaxLength) ||
                ReferenceEquals(declaration, CommonMetadata.Description);
        }

        public StringMetadata Resolve(MetadataRequest<StringMetadata> request)
        {
            return new StringMetadata(_MinLength, _MaxLength, _Description);
        }

        public string Resolve(MetadataRequest<string> request)
        {
            return _Description;
        }

        public int Resolve(MetadataRequest<int> request)
        {
            return request.Declaration == StringMetadata.MinLength ? _MinLength : _MaxLength;
        }
    }
}