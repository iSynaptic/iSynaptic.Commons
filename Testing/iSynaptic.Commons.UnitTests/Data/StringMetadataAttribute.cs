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

        public bool ProvidesMetadataFor<TMetadata, TSubject>(MetadataRequest<TMetadata, TSubject> request)
        {
            var declaration = request.Declaration;

            return
                ReferenceEquals(declaration, StringMetadata.All) ||
                ReferenceEquals(declaration, StringMetadata.MinLength) ||
                ReferenceEquals(declaration, StringMetadata.MaxLength) ||
                ReferenceEquals(declaration, CommonMetadata.Description);
        }

        public StringMetadata Resolve<TSubject>(MetadataRequest<StringMetadata, TSubject> request)
        {
            return new StringMetadata(_MinLength, _MaxLength, _Description);
        }

        public string Resolve<TSubject>(MetadataRequest<string, TSubject> request)
        {
            return _Description;
        }

        public int Resolve<TSubject>(MetadataRequest<int, TSubject> request)
        {
            return request.Declaration == StringMetadata.MinLength ? _MinLength : _MaxLength;
        }
    }
}