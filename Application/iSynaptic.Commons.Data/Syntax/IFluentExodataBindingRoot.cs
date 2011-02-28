using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentExodataBindingRoot
    {
        IFluentExodataBindingSubjectPredicateScopeTo<TExodata> Bind<TExodata>(IExodataDeclaration declaration);
        IFluentExodataBindingSubjectPredicateScopeTo<TExodata> Bind<TExodata>(IExodataDeclaration<TExodata> declaration);

        void Bind<TExodata>(IExodataDeclaration<TExodata> declaration, TExodata value);
        void Bind<TExodata>(IExodataDeclaration declaration, TExodata value);
    }

    public interface IFluentExodataBindingRoot<TSubject>
    {
        IFluentExodataBindingSpecificSubjectPredicateScopeTo<TExodata, TSubject> Bind<TExodata>(IExodataDeclaration declaration);
        IFluentExodataBindingSpecificSubjectPredicateScopeTo<TExodata, TSubject> Bind<TExodata>(IExodataDeclaration<TExodata> declaration);

        void Bind<TExodata>(IExodataDeclaration<TExodata> declaration, TExodata value);
        void Bind<TExodata>(IExodataDeclaration declaration, TExodata value);
    }
}
