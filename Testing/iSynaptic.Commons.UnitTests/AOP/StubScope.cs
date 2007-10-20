using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.UnitTests.AOP
{
    internal class StubScope : Scope<StubScope>
    {
        public static StubScope Current
        {
            get { return GetCurrent(); }
        }
    }
}
