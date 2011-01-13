using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public abstract class MetadataSurrogate<TSubject> : IMetadataBindingSource, IFluentInterface
    {
        private readonly MetadataBindingModule _Module = null;

        protected MetadataSurrogate()
        {
            _Module = new MetadataBindingModule();
        }

        public IPredicateScopeToBinding<TMetadata> Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration)
        {
            return _Module.Bind(declaration)
                .For<TSubject>();
        }

        public IPredicateScopeToBinding<TMetadata> Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration, TSubject subject)
        {
            return _Module.Bind(declaration)
                .For(subject);
        }

        public IPredicateScopeToBinding<TMetadata> Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration, Expression<Func<TSubject, object>> member)
        {
            return _Module.Bind(declaration)
                .For(member);
        }

        public IPredicateScopeToBinding<TMetadata> Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration, TSubject subject, Expression<Func<TSubject, object>> member)
        {
            return _Module.Bind(declaration)
                .For(subject, member);
        }

        IEnumerable<IMetadataBinding<TMetadata>> IMetadataBindingSource.GetBindingsFor<TMetadata>(MetadataRequest<TMetadata> request)
        {
            return _Module.GetBindingsFor(request);
        }
    }
}
