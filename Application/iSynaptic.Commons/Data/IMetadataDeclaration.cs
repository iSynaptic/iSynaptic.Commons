using System;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataDeclaration<in TMetadata>
    {
        void ValidateValue(TMetadata value);
    }
}