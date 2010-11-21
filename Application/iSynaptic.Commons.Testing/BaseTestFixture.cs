using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Testing
{
    public abstract class BaseTestFixture
    {
        protected Stream GetResource(string resourceName)
        {
            Assembly asm = Assembly.GetCallingAssembly();
            return asm.GetManifestResourceStream(resourceName);
        }

        protected virtual void BeforeTestFixture() { }
        protected virtual void AfterTestFixture() { }

        protected virtual void BeforeTest() { }
        protected virtual void AfterTest()  { }
    }
}
