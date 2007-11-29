using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.UnitTests.AOP
{
    public class StubScope : Scope<StubScope>
    {
        public static StubScope Current
        {
            get { return GetCurrentScope(); }
        }
    }
}
