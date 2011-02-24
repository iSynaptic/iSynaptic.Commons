using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DescriptionAttribute : Attribute, IMetadataAttribute<string>
    {
        private readonly string _Description;

        public DescriptionAttribute(string description)
        {
            _Description = description;
        }

        public bool ProvidesMetadataFor<TSubject>(IMetadataRequest<TSubject> request)
        {
            return request.Declaration == CommonMetadata.Description;
        }

        public string Resolve<TSubject>(IMetadataRequest<TSubject> request)
        {
            return _Description;
        }
    }
}
