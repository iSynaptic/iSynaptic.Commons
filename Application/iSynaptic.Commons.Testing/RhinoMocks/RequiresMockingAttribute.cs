using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Rhino.Mocks;

namespace iSynaptic.Commons.Testing.RhinoMocks
{
    public class RequiresMockingAttribute : Attribute, ITestBehavior
    {
        public void BeforeTest(object testFixture)
        {
            var mocksProperty = GetMocksPropertyDescriptor(testFixture);

            if (mocksProperty != null)
                mocksProperty.SetValue(testFixture, new MockRepository());
        }

        public void AfterTest(object testFixture)
        {
            var mocksProperty = GetMocksPropertyDescriptor(testFixture);

            if (mocksProperty != null)
            {
                var repo = (MockRepository)mocksProperty.GetValue(testFixture);
                mocksProperty.SetValue(testFixture, null);

                repo.VerifyAll();
            }
        }

        private static PropertyDescriptor GetMocksPropertyDescriptor(object testFixture)
        {
            var mocksProperty = TypeDescriptor.GetProperties(testFixture)
                .OfType<PropertyDescriptor>()
                .Where(x => x.Name == "Mocks" && x.PropertyType == typeof(MockRepository))
                .FirstOrDefault();

            return mocksProperty;
        }
    }
}
