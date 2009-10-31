using System;
using System.Collections.Generic;
using System.Text;

using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons.AOP
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
