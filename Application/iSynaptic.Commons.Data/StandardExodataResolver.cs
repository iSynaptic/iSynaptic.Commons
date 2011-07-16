using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public class StandardExodataResolver : ExodataResolver, IFluentExodataBindingRoot<object, object>
    {
        private readonly List<ExodataBindingModule> _Modules = new List<ExodataBindingModule>();
        private readonly ExodataBindingModule _ResolverModule = new ExodataBindingModule();

        private class ModuleExodataBindingSource : IExodataBindingSource
        {
            private readonly StandardExodataResolver _Parent;

            public ModuleExodataBindingSource(StandardExodataResolver parent)
            {
                Guard.NotNull(parent, "parent");
                _Parent = parent;
            }

            public IEnumerable<IExodataBinding> GetBindingsFor<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
            {
                return _Parent._Modules
                    .SelectMany(x => x.GetBindingsFor(request));
            }
        }

        public StandardExodataResolver(params ExodataBindingModule[] modules)
        {
            LoadModule(_ResolverModule);

            if (modules != null && modules.Length > 0)
            {
                foreach (var module in modules)
                    LoadModule(module);
            }

            AddExodataBindingSource(new ModuleExodataBindingSource(this));
            AddExodataBindingSource<SurrogateExodataBindingSource>();
            AddExodataBindingSource<AttributeExodataBindingSource>();
        }

        protected override int CompareBindingPrecidence<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request, IExodataBinding left, IExodataBinding right)
        {
            var l = left as IExodataBindingDetails;
            var r = right as IExodataBindingDetails;

            if (left == null || right == null)
                return 0;

            bool leftIsAttributeBinding = l.Source is AttributeExodataBindingSource;
            bool rightIsAttributeBinding = r.Source is AttributeExodataBindingSource;

            if (leftIsAttributeBinding ^ rightIsAttributeBinding)
                return leftIsAttributeBinding ? 1 : -1;

            bool leftBoundToContext = l.BoundToContextInstance;
            bool rightBoundToContext = r.BoundToContextInstance;

            if (leftBoundToContext ^ rightBoundToContext)
                return leftBoundToContext ? -1 : 1;

            bool leftBoundToSubject = l.BoundToSubjectInstance;
            bool rightBoundToSubject = r.BoundToSubjectInstance;

            if (leftBoundToSubject ^ rightBoundToSubject)
                return leftBoundToSubject ? -1 : 1;

            if (l.ContextType != r.ContextType)
                return l.ContextType.IsAssignableFrom(r.ContextType) ? 1 : -1;

            if (l.SubjectType != r.SubjectType)
                return l.SubjectType.IsAssignableFrom(r.SubjectType) ? 1 : -1;

            return 0;
        }


        public void LoadModule(ExodataBindingModule module)
        {
            Guard.NotNull(module, "module");
            _Modules.Add(module);
        }

        public void UnloadModule(ExodataBindingModule module)
        {
            Guard.NotNull(module, "module");
            _Modules.Remove(module);
        }

        public IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, object, object> Bind<TExodata>(ISymbol<TExodata> symbol)
        {
            return _ResolverModule.Bind(symbol);
        }

        public void Bind<TExodata>(ISymbol<TExodata> symbol, TExodata value, string name = null)
        {
            Bind(symbol).Named(name).To(value);
        }
    }
}
