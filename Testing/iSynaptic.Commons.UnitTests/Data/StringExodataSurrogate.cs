using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class StringExodataSurrogate : ExodataSurrogate<string>
    {
        public StringExodataSurrogate()
        {
            Bind(CommonExodata.Description, "A string...");
        }
    }
}
