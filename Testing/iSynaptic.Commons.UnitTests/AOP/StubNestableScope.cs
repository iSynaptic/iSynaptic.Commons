using System;
using System.Collections.Generic;
using System.Text;

using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons.AOP
{
    public class StubNestableScope : NestableScope<StubNestableScope>
    {
        public StubNestableScope()
        {
        }

        public StubNestableScope(ScopeBounds bounds) : base(bounds)
        {
        }

        public static StubNestableScope Current
        {
            get { return NestableScope<StubNestableScope>.GetCurrentScope(); }
        }
    }
}
