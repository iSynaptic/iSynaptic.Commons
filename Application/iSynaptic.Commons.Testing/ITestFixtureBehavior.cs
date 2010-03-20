using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Testing
{
    public interface ITestFixtureBehavior
    {
        void BeforeTestFixture(object testFixture);
        void AfterTestFixture(object testFixture);
    }
}
