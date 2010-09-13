using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Reflection;

namespace iSynaptic.Commons.Testing
{
    public abstract class BaseTestFixture
    {
        private IEnumerable<ITestFixtureBehavior> _FixtureBehaviors = null;

        protected Stream GetResource(string resourceName)
        {
            Assembly asm = Assembly.GetCallingAssembly();
            return asm.GetManifestResourceStream(resourceName);
        }

        protected virtual void BeforeTestFixture()
        {
            _FixtureBehaviors = GetType().GetAttributesOfType<ITestFixtureBehavior>()
                .ToArray();

            foreach (var behavior in _FixtureBehaviors)
                behavior.BeforeTestFixture(this);
        }

        protected virtual void AfterTestFixture()
        {
            foreach (var behavior in _FixtureBehaviors)
                behavior.AfterTestFixture(this);
        }

        protected virtual void BeforeTest() { }
        protected virtual void AfterTest()  { }
    }
}
