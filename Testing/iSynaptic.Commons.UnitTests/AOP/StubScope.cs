using System;
using System.Collections.Generic;
using System.Text;

using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons.AOP
{
    public class StubScope : Scope<StubScope>
    {
        public StubScope(ScopeBounds bounds, ScopeNesting nesting) : base(bounds, nesting)
        {
        }

        public static StubScope Current
        {
            get { return GetCurrentScope(); }
        }
    }
}
