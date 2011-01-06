using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataBinding<TMetadata> : IMetadataBinding<TMetadata>
    {
        private readonly IMetadataDeclaration<TMetadata> _Declaration;
        private readonly object _Value;

        public MetadataBinding(IMetadataDeclaration<TMetadata> declaration, object value)
        {
            Guard.NotNull(declaration, "declaration");

            _Declaration = declaration;
            _Value = value;
        }

        public bool Matches(MetadataRequest<TMetadata> request)
        {
            return request.Declaration == _Declaration;
        }

        public Func<MetadataRequest<TMetadata>, object> ScopeFactory
        {
            get { return null; }
        }

        public TMetadata Resolve(MetadataRequest<TMetadata> request)
        {
            return (TMetadata)_Value;
        }
    }
}
