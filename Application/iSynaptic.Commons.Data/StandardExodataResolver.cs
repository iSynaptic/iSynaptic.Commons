using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public class StandardExodataResolver : ExodataResolver, IFluentExodataBindingRoot
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

            public IEnumerable<IExodataBinding> GetBindingsFor<TExodata, TSubject>(IExodataRequest<TSubject> request)
            {
                return _Parent._Modules
                    .SelectMany(x => x.GetBindingsFor<TExodata, TSubject>(request));
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

        protected override IExodataBinding SelectBinding<TExodata, TSubject>(IExodataRequest<TSubject> request, IEnumerable<IExodataBinding> candidates)
        {
            var bindingList = candidates
                .ToList();

            if (bindingList.Count > 1)
            {
                bindingList.Sort(BindingSortPriority);
                if (BindingSortPriority(bindingList[0], bindingList[1]) != 0)
                    return bindingList[0];
            }

            return base.SelectBinding<TExodata, TSubject>(request, bindingList);
        }

        private static int BindingSortPriority(IExodataBinding left, IExodataBinding right)
        {
            if (!(left.Source is AttributeExodataBindingSource) && right.Source is AttributeExodataBindingSource)
                return -1;

            if (!(right.Source is AttributeExodataBindingSource) && left.Source is AttributeExodataBindingSource)
                return 1;

            if (left.BoundToSubjectInstance && !right.BoundToSubjectInstance)
                return -1;

            if (right.BoundToSubjectInstance && !left.BoundToSubjectInstance)
                return 1;

            if(left.SubjectType != right.SubjectType)
            {
                if (left.SubjectType.IsAssignableFrom(right.SubjectType))
                    return 1;

                return -1;
            }

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

        public IFluentExodataBindingSubjectPredicateScopeTo<TExodata> Bind<TExodata>(IExodataDeclaration declaration)
        {
            return _ResolverModule.Bind<TExodata>(declaration);
        }

        public IFluentExodataBindingSubjectPredicateScopeTo<TExodata> Bind<TExodata>(IExodataDeclaration<TExodata> declaration)
        {
            return _ResolverModule.Bind(declaration);
        }

        public void Bind<TExodata>(IExodataDeclaration<TExodata> declaration, TExodata value)
        {
            _ResolverModule.Bind(declaration, value);
        }

        public void Bind<TExodata>(IExodataDeclaration declaration, TExodata value)
        {
            _ResolverModule.Bind(declaration, value);
        }
    }
}
