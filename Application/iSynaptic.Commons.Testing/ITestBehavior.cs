using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Testing
{
    public interface ITestBehavior
    {
        void BeforeTest(object testFixture);
        void AfterTest(object testFixture);
    }
}
