using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.Data.MetadataDeclarations;

namespace iSynaptic.Commons.Data
{
    public static class CommonMetadata
    {
        public readonly static MetadataDeclaration<CommonMetadataDefinition> All = new MetadataDeclaration<CommonMetadataDefinition>();

        public static readonly StringMetadataDeclaration Description = new StringMetadataDeclaration();
    }

    public class CommonMetadataDefinition
    {
        public CommonMetadataDefinition(string description)
        {
            Description = description;
        }

        public string Description { get; private set; }
    }
}
