using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataBinding : IMetadataBinding
    {
        private readonly IMetadataDeclaration _Declaration;
        private readonly object _Value;

        public MetadataBinding(IMetadataDeclaration declaration, object value)
        {
            _Declaration = declaration;
            _Value = value;
        }

        public bool Matches(MetadataRequest request)
        {
            return request.Declaration == _Declaration;
        }

        public Func<MetadataRequest, object> ScopeFactory
        {
            get { throw new NotImplementedException(); }
        }

        public T Resolve<T>(MetadataRequest request)
        {
            return (T) _Value;
        }
    }
}
