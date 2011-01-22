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

        public bool ProvidesMetadataFor<TSubject>(IMetadataRequest<TSubject> request)
        {
            return
                request.Declaration == StringMetadata.All ||
                request.Declaration == StringMetadata.MinLength ||
                request.Declaration == StringMetadata.MaxLength ||
                request.Declaration == CommonMetadata.Description;
        }

        StringMetadata IMetadataAttribute<StringMetadata>.Resolve<TSubject>(IMetadataRequest<TSubject> request)
        {
            return new StringMetadata(_MinLength, _MaxLength, _Description);
        }

        string IMetadataAttribute<string>.Resolve<TSubject>(IMetadataRequest<TSubject> request)
        {
            return _Description;
        }

        int IMetadataAttribute<int>.Resolve<TSubject>(IMetadataRequest<TSubject> request)
        {
            return request.Declaration == StringMetadata.MinLength ? _MinLength : _MaxLength;
        }
    }
}