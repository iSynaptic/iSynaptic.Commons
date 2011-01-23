﻿using System;
using System.Reflection.Emit;

namespace iSynaptic.Commons.Data
{
    public class StringMetadataAttribute : Attribute, IMetadataAttribute<int>, IMetadataAttribute<string>, IMetadataAttribute<StringMetadataDefinition>
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

        public bool ProvidesMetadataFor<TMetadata, TSubject>(IMetadataRequest<TMetadata, TSubject> request)
        {
            return
                request.Declaration == StringMetadata.All ||
                request.Declaration == StringMetadata.MinLength ||
                request.Declaration == StringMetadata.MaxLength ||
                request.Declaration == CommonMetadata.All ||
                request.Declaration == CommonMetadata.Description;
        }

        public StringMetadataDefinition Resolve<TSubject>(IMetadataRequest<StringMetadataDefinition, TSubject> request)
        {
            return new StringMetadataDefinition(_MinLength, _MaxLength, _Description);
        }

        public string Resolve<TSubject>(IMetadataRequest<string, TSubject> request)
        {
            return _Description;
        }

        public int Resolve<TSubject>(IMetadataRequest<int, TSubject> request)
        {
            return request.Declaration == StringMetadata.MinLength ? _MinLength : _MaxLength;
        }
    }
}