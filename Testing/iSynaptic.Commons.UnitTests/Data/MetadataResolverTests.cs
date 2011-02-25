using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.Data.MetadataDeclarations;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class MetadataResolverTests
    {
        [Test]
        public void Resolve_WithNoBindings_ReturnsDefault()
        {
            var resolver = new MetadataResolver();
            MetadataDeclaration.SetResolver(resolver);

            var declaration = new ComparableMetadataDeclaration<int>(-1, 42, 7);
            var value = declaration.Get();

            Assert.AreEqual(7, value);
        }

        [Test]
        public void Resolve_WithAmbiguousBindingSelection_ThrowsException()
        {
            var binding1 = MockRepository.GenerateStub<IMetadataBinding>();
            var binding2 = MockRepository.GenerateStub<IMetadataBinding>();

            Action<IMetadataBinding> expectations = b =>
                b.Expect(x => x.Matches<int, object>(null))
                    .IgnoreArguments()
                    .Return(true);

            expectations(binding1);
            expectations(binding2);

            var source = MockRepository.GenerateStub<IMetadataBindingSource>();
            source.Expect(x => x.GetBindingsFor<int, object>(null))
                .IgnoreArguments()
                .Return(new[] { binding1, binding2 });

            var resolver = new MetadataResolver();
            resolver.AddMetadataBindingSource(source);

            MetadataDeclaration.SetResolver(resolver);
            Assert.Throws<InvalidOperationException>(() => StringMetadata.MaxLength.Get());
        }

        [Test]
        public void Resolve_WithScopeFactoryObject_CachesResult()
        {
            int resolveCount = 0;
            var scopeObject = new object();

            var resolver = new StandardMetadataResolver();
            resolver.Bind(StringMetadata.MaxLength)
                .InScope(x => scopeObject)
                .To(r => { resolveCount++; return 42; });

            MetadataDeclaration.SetResolver(resolver);

            int maxLength = StringMetadata.MaxLength;
            Assert.AreEqual(42, maxLength);

            maxLength = StringMetadata.MaxLength;
            Assert.AreEqual(42, maxLength);

            maxLength = StringMetadata.MaxLength;
            Assert.AreEqual(42, maxLength);
            
            Assert.AreEqual(1, resolveCount);
        }

        [Test]
        public void Resolve_WithScopeFactoryObject_FlushesCacheCorrectly()
        {
            int resolveCount = 0;
            var scopeObject = new object();

            var resolver = new StandardMetadataResolver();
            resolver.Bind(StringMetadata.MaxLength)
                .InScope(x => scopeObject)
                .To(r => { resolveCount++; return 42; });

            MetadataDeclaration.SetResolver(resolver);

            int maxLength = StringMetadata.MaxLength;
            Assert.AreEqual(42, maxLength);

            maxLength = StringMetadata.MaxLength;
            Assert.AreEqual(42, maxLength);

            scopeObject = null;
            GC.Collect();

            maxLength = StringMetadata.MaxLength;
            Assert.AreEqual(42, maxLength);
            Assert.AreEqual(2, resolveCount);
        }
    }
}
