using System;
using System.Collections.Generic;
using System.Text;

using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons.UnitTests.AOP
{
    public class UnitOfWorkStub : UnitOfWork<UnitOfWorkStub, object>
    {
        public override void Complete()
        {
        }

        public List<object> GetItems()
        {
            return Items;
        }
    }
}
