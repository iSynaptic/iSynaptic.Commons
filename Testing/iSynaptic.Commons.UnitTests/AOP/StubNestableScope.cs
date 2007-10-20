using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.UnitTests.AOP
{
    public class StubNestableScope : NestableScope<StubNestableScope>
    {
        public StubNestableScope() : base()
        {
        }

        public StubNestableScope(ScopeBounds bounds) : base(bounds)
        {
        }

        public static StubNestableScope Current
        {
            get { return GetCurrent(); }
        }
    }
}
