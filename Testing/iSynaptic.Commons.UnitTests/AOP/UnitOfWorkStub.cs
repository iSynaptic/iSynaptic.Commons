using System;
using System.Collections.Generic;
using System.Text;

using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons.UnitTests.AOP
{
    public class UnitOfWorkStub : UnitOfWork<UnitOfWorkStub, object>
    {
        protected override void Process(object item)
        {
        }

        public List<object> GetItems()
        {
            return Items;
        }
    }
}
