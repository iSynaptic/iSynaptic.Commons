using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Core.Extensibility;

namespace iSynaptic.Commons.Testing.NUnit.Addins
{
    [NUnitAddin(Name = "iSynaptic Commons Behavioral Attributes Addin")]
    public class BehavioralAttributeAddin : IAddin
    {
        public bool Install(IExtensionHost host)
        {
            IExtensionPoint testDecorators = host.GetExtensionPoint("TestDecorators");
            testDecorators.Install(new BehavioralAttributeTestDecorator());

            return true;
        }
    }
}
