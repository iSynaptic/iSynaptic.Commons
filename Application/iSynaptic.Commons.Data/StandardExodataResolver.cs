using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public class StandardExodataResolver : ExodataResolver, IFluentExodataBindingRoot<object, object>
    {
        private readonly HashSet<IExodataBindingSource> _BindingSources = new HashSet<IExodataBindingSource>();

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

        public override IEnumerable<IExodataBindingSource> GetBindingSources()
        {
            return _BindingSources;
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

        public T AddExodataBindingSource<T>() where T : IExodataBindingSource, new()
        {
            T source = new T();
            AddExodataBindingSource(source);

            return source;
        }

        public void AddExodataBindingSource(IExodataBindingSource source)
        {
            Guard.NotNull(source, "source");

            _BindingSources.Add(source);
        }

        public void RemoveExodataBindingSource(IExodataBindingSource source)
        {
            Guard.NotNull(source, "source");
            _BindingSources.Remove(source);
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

        #region IFluentExodataBindingRoot Members

        public IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, object, object> Bind<TExodata>(ISymbol<TExodata> symbol)
        {
            return Bind<TExodata>((ISymbol) symbol);
        }

        public IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, object, object> Bind<TExodata>(ISymbol symbol)
        {
            return _ResolverModule.Bind<TExodata>(symbol);
        }

        public void Bind<TExodata>(ISymbol<TExodata> symbol, TExodata value, string name = null)
        {
            Bind((ISymbol) symbol, value, name);
        }

        public void Bind<TExodata>(ISymbol symbol, TExodata value, string name = null)
        {
            Bind<TExodata>(symbol).Named(name).To(value);
        }

        #endregion
    }
}
