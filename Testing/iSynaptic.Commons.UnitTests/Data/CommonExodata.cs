using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.Data.ExodataDeclarations;

namespace iSynaptic.Commons.Data
{
    public static class CommonExodata
    {
        public readonly static ExodataDeclaration<CommonExodataDefinition> All = new ExodataDeclaration<CommonExodataDefinition>();

        public static readonly StringExodataDeclaration Description = new StringExodataDeclaration();
    }

    public class CommonExodataDefinition
    {
        public CommonExodataDefinition(string description)
        {
            Description = description;
        }

        public string Description { get; private set; }
    }
}
