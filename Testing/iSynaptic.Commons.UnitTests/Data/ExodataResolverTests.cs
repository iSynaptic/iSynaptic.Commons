using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class ExodataResolverTests
    {
        [Test]
        public void TryResolve_WithNoBindings_ReturnsNoValue()
        {
            var resolver = new ExodataResolver();
            var symbol = new Symbol<int>();

            var value = resolver.TryGet(symbol);

            Assert.AreEqual(Maybe<int>.NoValue, value);
        }

        [Test]
        public void TryResolve_WithAmbiguousBindingSelection_ThrowsException()
        {
            var binding1 = MockRepository.GenerateStub<IExodataBinding>();
            var binding2 = MockRepository.GenerateStub<IExodataBinding>();

            Action<IExodataBinding> expectations = b =>
                b.Expect(x => x.TryResolve<int, object, object>(null))
                    .IgnoreArguments()
                    .Return(42.ToMaybe());

            expectations(binding1);
            expectations(binding2);

            var source = MockRepository.GenerateStub<IExodataBindingSource>();
            source.Expect(x => x.GetBindingsFor<int, object, object>(null))
                .IgnoreArguments()
                .Return(new[] { binding1, binding2 });

            var resolver = new ExodataResolver();
            resolver.AddExodataBindingSource(source);

            var symbol = new Symbol<int>();

            var result = resolver.TryGet(symbol);

            Assert.IsFalse(result.HasValue);
            Assert.IsNotNull(result.Exception);
            Assert.IsInstanceOf<InvalidOperationException>(result.Exception);
        }
    }
}
