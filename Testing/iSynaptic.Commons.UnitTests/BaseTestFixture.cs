using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using NUnit.Framework;

namespace iSynaptic.Commons.UnitTests
{
    public abstract class BaseTestFixture
    {
        public Stream GetResource(string resourceName)
        {
            Assembly asm = Assembly.GetCallingAssembly();
            return asm.GetManifestResourceStream(resourceName);
        }
    }
}
