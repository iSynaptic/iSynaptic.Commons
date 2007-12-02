using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.UnitTests.AOP
{
    public class StubScope : Scope<StubScope>
    {
        public StubScope()
        {
        }

        public StubScope(ScopeBounds bounds) : base(bounds)
        {
        }

        public static StubScope Current
        {
            get { return GetCurrentScope(); }
        }
    }
}
