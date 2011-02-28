using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public abstract class ExodataSurrogate<TSubject> : IExodataBindingSource, IFluentExodataBindingRoot<TSubject>, IFluentInterface
    {
        private readonly HashSet<IExodataBinding> _Bindings = new HashSet<IExodataBinding>();

        IEnumerable<IExodataBinding> IExodataBindingSource.GetBindingsFor<TExodata, TBindingSubject>(IExodataRequest<TBindingSubject> request)
        {
            return _Bindings;
        }

        public IFluentExodataBindingSpecificSubjectPredicateScopeTo<TExodata, TSubject> Bind<TExodata>(IExodataDeclaration declaration)
        {
            Guard.NotNull(declaration, "declaration");
            return new FluentExodataBindingBuilder<TExodata, TSubject>(this, declaration, b => _Bindings.Add(b));
        }

        public IFluentExodataBindingSpecificSubjectPredicateScopeTo<TExodata, TSubject> Bind<TExodata>(IExodataDeclaration<TExodata> declaration)
        {
            return Bind<TExodata>((IExodataDeclaration)declaration);
        }

        public void Bind<TExodata>(IExodataDeclaration<TExodata> declaration, TExodata value)
        {
            Bind<TExodata>((IExodataDeclaration)declaration)
                .To(value);
        }

        public void Bind<TExodata>(IExodataDeclaration declaration, TExodata value)
        {
            Bind<TExodata>(declaration)
                .To(value);
        }
    }
}
