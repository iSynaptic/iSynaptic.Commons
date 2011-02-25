using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentMetadataBindingRoot
    {
        IFluentMetadataBindingSubjectPredicateScopeTo<TMetadata> Bind<TMetadata>(IMetadataDeclaration declaration);
        IFluentMetadataBindingSubjectPredicateScopeTo<TMetadata> Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration);

        void Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration, TMetadata value);
        void Bind<TMetadata>(IMetadataDeclaration declaration, TMetadata value);
    }

    public interface IFluentMetadataBindingRoot<TSubject>
    {
        IFluentMetadataBindingSpecificSubjectPredicateScopeTo<TMetadata, TSubject> Bind<TMetadata>(IMetadataDeclaration declaration);
        IFluentMetadataBindingSpecificSubjectPredicateScopeTo<TMetadata, TSubject> Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration);

        void Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration, TMetadata value);
        void Bind<TMetadata>(IMetadataDeclaration declaration, TMetadata value);
    }
}
