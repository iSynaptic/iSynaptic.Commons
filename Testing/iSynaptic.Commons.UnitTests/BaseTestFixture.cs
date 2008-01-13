using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using MbUnit.Core.Exceptions;
using System.IO;
using System.Reflection;

namespace iSynaptic.Commons.UnitTests
{
    public abstract class BaseTestFixture
    {
        public void AssertThrows(Action action)
        {
            bool exceptionThrown = false;
            try
            {
                action();
            }
            catch
            {
                exceptionThrown = true;
            }

            if(exceptionThrown != true)
                Assert.Fail("Exception was not thrown.");
        }

        public void AssertThrows<T>(Action action)
        {
            bool exceptionThrown = false;

            try
            {
                action();
                            }
            catch (Exception ex)
            {
                if (ex is T != true)
                    Assert.Fail("Expected exception of type '{0}'; instead exception was of type '{1}'", typeof(T).Name, ex.GetType().Name);

                exceptionThrown = true;
            }

            if (exceptionThrown != true)
                Assert.Fail("Expected exception of type '{0}'; however no exception was thrown.", typeof(T).Name);
        }

        public Stream GetResource(string resourceName)
        {
            Assembly asm = Assembly.GetCallingAssembly();
            return asm.GetManifestResourceStream(resourceName);
        }
    }
}
