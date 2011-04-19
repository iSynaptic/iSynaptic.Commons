using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.Data.ExodataDeclarations;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class ExodataResolverTests
    {
        [Test]
        public void Resolve_WithNoBindings_ReturnsDefault()
        {
            var resolver = new ExodataResolver();
            ExodataDeclaration.SetResolver(resolver);

            var declaration = new ComparableExodataDeclaration<int>(-1, 42, 7);
            var value = declaration.Get();

            Assert.AreEqual(7, value);
        }

        [Test]
        public void Resolve_WithAmbiguousBindingSelection_ThrowsException()
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

            ExodataDeclaration.SetResolver(resolver);
            Assert.Throws<InvalidOperationException>(() => StringExodata.MaxLength.Get());
        }

        [Test]
        public void Resolve_WithScopeFactoryObject_CachesResult()
        {
            int resolveCount = 0;
            var scopeObject = new object();

            var resolver = new StandardExodataResolver();
            resolver.Bind(StringExodata.MaxLength)
                .InScope(x => scopeObject)
                .To(r => { resolveCount++; return 42; });

            ExodataDeclaration.SetResolver(resolver);

            int maxLength = StringExodata.MaxLength;
            Assert.AreEqual(42, maxLength);

            maxLength = StringExodata.MaxLength;
            Assert.AreEqual(42, maxLength);

            maxLength = StringExodata.MaxLength;
            Assert.AreEqual(42, maxLength);
            
            Assert.AreEqual(1, resolveCount);
        }

        [Test]
        public void Resolve_WithScopeFactoryObject_FlushesCacheCorrectly()
        {
            int resolveCount = 0;
            var scopeObject = new object();

            var resolver = new StandardExodataResolver();
            resolver.Bind(StringExodata.MaxLength)
                .InScope(x => scopeObject)
                .To(r => { resolveCount++; return 42; });

            ExodataDeclaration.SetResolver(resolver);

            int maxLength = StringExodata.MaxLength;
            Assert.AreEqual(42, maxLength);

            maxLength = StringExodata.MaxLength;
            Assert.AreEqual(42, maxLength);

            scopeObject = null;
            GC.Collect();

            maxLength = StringExodata.MaxLength;
            Assert.AreEqual(42, maxLength);
            Assert.AreEqual(2, resolveCount);
        }

        [Test]
        public void Resolve_WithExodataScopeObject_ChecksToSeeIfExodataScopeObjectSaysItIsInScope()
        {
            int resolveCount = 0;

            bool isInScope = true;

            var exodataScopeObject = MockRepository.GenerateStub<IExodataScopeObject>();
            exodataScopeObject.Expect(x => x.IsInScope<int, object, object>(null, null))
                .IgnoreArguments()
                .Do((Func<IExodataBinding, IExodataRequest<object, object>, bool>) ((b, r) => isInScope));

            var resolver = new StandardExodataResolver();
            resolver.Bind(StringExodata.MaxLength)
                .InScope(x => exodataScopeObject)
                .To(r => { resolveCount++; return 42; });

            ExodataDeclaration.SetResolver(resolver);

            int maxLength = StringExodata.MaxLength;
            Assert.AreEqual(42, maxLength);

            maxLength = StringExodata.MaxLength;
            Assert.AreEqual(42, maxLength);

            isInScope = false;

            maxLength = StringExodata.MaxLength;

            Assert.AreEqual(42, maxLength);
            Assert.AreEqual(2, resolveCount);
        }

        [Test]
        public void Resolve_WithExodataScopeObject_FlushesCacheWhenScopeRequestsIt()
        {
            int resolveCount = 0;

            var exodataScopeObject = MockRepository.GenerateStub<IExodataScopeObject>();
            exodataScopeObject.Expect(x => x.IsInScope<int, object, object>(null, null))
                .IgnoreArguments()
                .Return(true);

            var resolver = new StandardExodataResolver();
            resolver.Bind(StringExodata.MaxLength)
                .InScope(x => exodataScopeObject)
                .To(r => { resolveCount++; return 42; });

            ExodataDeclaration.SetResolver(resolver);

            int maxLength = StringExodata.MaxLength;
            Assert.AreEqual(42, maxLength);

            maxLength = StringExodata.MaxLength;
            Assert.AreEqual(42, maxLength);

            exodataScopeObject.Raise(x => x.CacheFlushRequested += null, this, EventArgs.Empty);

            maxLength = StringExodata.MaxLength;

            Assert.AreEqual(42, maxLength);
            Assert.AreEqual(2, resolveCount);
        }
    }
}
