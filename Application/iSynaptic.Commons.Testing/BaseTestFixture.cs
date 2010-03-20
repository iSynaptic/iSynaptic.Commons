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
        private IEnumerable<ITestFixtureBehavior> _FixtureBehaviors = null;
        private IEnumerable<ITestBehavior> _TestBehaviors = null;

        protected Stream GetResource(string resourceName)
        {
            Assembly asm = Assembly.GetCallingAssembly();
            return asm.GetManifestResourceStream(resourceName);
        }

        protected virtual void BeforeTestFixture()
        {
            _FixtureBehaviors = GetTypeAttributes<ITestFixtureBehavior>()
                .ToArray();

            foreach (var behavior in _FixtureBehaviors)
                behavior.BeforeTestFixture(this);
        }

        protected virtual void AfterTestFixture()
        {
            foreach (var behavior in _FixtureBehaviors)
                behavior.AfterTestFixture(this);
        }

        protected virtual void BeforeTest()
        {
            _TestBehaviors = GetTypeAttributes<ITestBehavior>()
                .ToArray();

            foreach (var behavior in _TestBehaviors)
                behavior.BeforeTest(this);
        }

        protected virtual void AfterTest()
        {
            foreach (var behavior in _TestBehaviors)
                behavior.AfterTest(this);
        }

        protected IEnumerable<T> GetTypeAttributes<T>()
        {
            Type fixtureType = GetType();
            return fixtureType.GetCustomAttributes(true)
                .Where(x => typeof(T).IsAssignableFrom(x.GetType()))
                .Cast<T>();
        }
    }
}
