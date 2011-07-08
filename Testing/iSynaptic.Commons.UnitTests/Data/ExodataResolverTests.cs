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
                b.Expect(x => x.Matches<int, object, object>(null))
                    .IgnoreArguments()
                    .Return(true);

            expectations(binding1);
            expectations(binding2);

            var source = MockRepository.GenerateStub<IExodataBindingSource>();
            source.Expect(x => x.GetBindingsFor<int, object, object>(null))
                .IgnoreArguments()
                .Return(new[] { binding1, binding2 });

            var resolver = new ExodataResolver();
            resolver.AddExodataBindingSource(source);

            var symbol = new Symbol<int>();

            Assert.Throws<InvalidOperationException>(() => resolver.TryGet(symbol));
        }

        [Test]
        public void TryResolve_WithScopeFactoryObject_CachesResult()
        {
            int resolveCount = 0;
            var scopeObject = new object();
            var symbol = new Symbol<int>();

            var resolver = new StandardExodataResolver();

            resolver.Bind(symbol)
                .InScope(x => scopeObject)
                .To(r => { resolveCount++; return 42; });

            int maxLength = resolver.TryGet(symbol).Value;
            Assert.AreEqual(42, maxLength);

            maxLength = resolver.TryGet(symbol).Value;
            Assert.AreEqual(42, maxLength);

            maxLength = resolver.TryGet(symbol).Value;
            Assert.AreEqual(42, maxLength);
            
            Assert.AreEqual(1, resolveCount);
        }

        [Test]
        public void TryResolve_WithScopeFactoryObject_FlushesCacheCorrectly()
        {
            int resolveCount = 0;
            var scopeObject = new object();
            var symbol = new Symbol<int>();

            var resolver = new StandardExodataResolver();
            resolver.Bind(symbol)
                .InScope(x => scopeObject)
                .To(r => { resolveCount++; return 42; });

            int maxLength = resolver.TryGet(symbol).Value;
            Assert.AreEqual(42, maxLength);

            maxLength = resolver.TryGet(symbol).Value;
            Assert.AreEqual(42, maxLength);

            scopeObject = null;
            GC.Collect();

            maxLength = resolver.TryGet(symbol).Value;

            Assert.AreEqual(42, maxLength);
            Assert.AreEqual(2, resolveCount);
        }

        [Test]
        public void TryResolve_WithExodataScopeObject_ChecksToSeeIfExodataScopeObjectSaysItIsInScope()
        {
            int resolveCount = 0;
            var symbol = new Symbol<int>();

            bool isInScope = true;

            var exodataScopeObject = MockRepository.GenerateStub<IExodataScopeObject>();
            exodataScopeObject.Expect(x => x.IsInScope<int, object, object>(null, null))
                .IgnoreArguments()
                .Do((Func<IExodataBinding, IExodataRequest<int, object, object>, bool>) ((b, r) => isInScope));

            var resolver = new StandardExodataResolver();

            resolver.Bind(symbol)
                .InScope(x => exodataScopeObject)
                .To(r => { resolveCount++; return 42; });

            int maxLength = resolver.TryGet(symbol).Value;
            Assert.AreEqual(42, maxLength);

            maxLength = resolver.TryGet(symbol).Value;
            Assert.AreEqual(42, maxLength);

            isInScope = false;

            maxLength = resolver.TryGet(symbol).Value;

            Assert.AreEqual(42, maxLength);
            Assert.AreEqual(2, resolveCount);
        }

        [Test]
        public void TryResolve_WithExodataScopeObject_FlushesCacheWhenScopeRequestsIt()
        {
            int resolveCount = 0;
            var symbol = new Symbol<int>();

            var exodataScopeObject = MockRepository.GenerateStub<IExodataScopeObject>();
            exodataScopeObject.Expect(x => x.IsInScope<int, object, object>(null, null))
                .IgnoreArguments()
                .Return(true);

            var resolver = new StandardExodataResolver();
            resolver.Bind(symbol)
                .InScope(x => exodataScopeObject)
                .To(r => { resolveCount++; return 42; });

            int maxLength = resolver.TryGet(symbol).Value;
            Assert.AreEqual(42, maxLength);

            maxLength = resolver.TryGet(symbol).Value;
            Assert.AreEqual(42, maxLength);

            exodataScopeObject.Raise(x => x.CacheFlushRequested += null, this, EventArgs.Empty);

            maxLength = resolver.TryGet(symbol).Value;

            Assert.AreEqual(42, maxLength);
            Assert.AreEqual(2, resolveCount);
        }
    }
}
