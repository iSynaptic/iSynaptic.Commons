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
            var value = declaration.For<object>();

            Assert.AreEqual(7, value);
        }

        [Test]
        public void Resolve_ThruImplicitCastOperator_ReturnsValue()
        {
            var binding = new MetadataBinding<int, object>(r => r.Declaration == StringMetadata.MaxLength, r => 42, MockRepository.GenerateStub<IMetadataBindingSource>());

            var source = MockRepository.GenerateStub<IMetadataBindingSource>();
            source.Expect(x => x.GetBindingsFor<int, object>(null))
                .IgnoreArguments()
                .Return(new[] { binding });

            var resolver = new MetadataResolver();
            resolver.AddMetadataBindingSource(source);

            MetadataDeclaration.SetResolver(resolver);

            int value = StringMetadata.MaxLength;
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Resolve_WithAmbiguousBindingSelection_ThrowsException()
        {
            var binding1 = MockRepository.GenerateStub<IMetadataBinding<int, object>>();
            var binding2 = MockRepository.GenerateStub<IMetadataBinding<int, object>>();

            Action<IMetadataBinding<int, object>> expectations = b =>
                b.Expect(x => x.Matches(null))
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

            var binding = new MetadataBinding<int, object>(r => r.Declaration == StringMetadata.MaxLength, x => { resolveCount++; return 42; }, MockRepository.GenerateStub<IMetadataBindingSource>()) { ScopeFactory = x => scopeObject };

            var source = MockRepository.GenerateStub<IMetadataBindingSource>();
            source.Expect(x => x.GetBindingsFor<int, object>(null))
                .IgnoreArguments()
                .Return(new[] { binding });

            var resolver = new MetadataResolver();
            resolver.AddMetadataBindingSource(source);

            MetadataDeclaration.SetResolver(resolver);

            int maxLength = StringMetadata.MaxLength;
            maxLength = StringMetadata.MaxLength;
            maxLength = StringMetadata.MaxLength;

            Assert.AreEqual(1, resolveCount);
        }

        [Test]
        public void Resolve_WithScopeFactoryObject_FlushesCacheCorrectly()
        {
            int resolveCount = 0;
            var scopeObject = new object();

            var binding = new MetadataBinding<int, object>(r => r.Declaration == StringMetadata.MaxLength, x => { resolveCount++; return 42; }, MockRepository.GenerateStub<IMetadataBindingSource>()) { ScopeFactory = x => scopeObject };

            var source = MockRepository.GenerateStub<IMetadataBindingSource>();
            source.Expect(x => x.GetBindingsFor<int, object>(null))
                .IgnoreArguments()
                .Return(new[] { binding });

            var resolver = new MetadataResolver();
            resolver.AddMetadataBindingSource(source);

            MetadataDeclaration.SetResolver(resolver);

            int maxLength = StringMetadata.MaxLength;
            maxLength = StringMetadata.MaxLength;

            scopeObject = null;
            GC.Collect();

            maxLength = StringMetadata.MaxLength;
            Assert.AreEqual(2, resolveCount);
        }
    }
}
