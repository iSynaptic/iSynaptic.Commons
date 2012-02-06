using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class AttributeExodataBindingSourceTests
    {
        [Test]
        public void SourceReturnsNoBinding_GivenNoSubjectOrMember()
        {
            var source = new AttributeExodataBindingSource();
            var bindings = source.GetBindingsFor(ExodataRequest.Create<object, object, object>(new Symbol(), Maybe.NoValue, Maybe.NoValue, null));

            Assert.IsNotNull(bindings);
            Assert.AreEqual(0, bindings.Count());
        }
    }
}
