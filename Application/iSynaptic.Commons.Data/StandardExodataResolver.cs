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

        protected override IExodataBinding SelectBinding<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request, IEnumerable<IExodataBinding> candidates)
        {
            var bindingList = candidates.ToList();
            bindingList.Sort(BindingPrecedenceOrder);

            Func<IExodataRequest<TExodata, TContext, TSubject>, IEnumerable<IExodataBinding>, IExodataBinding>
                fallThroughSelection = base.SelectBinding;

            return Maybe.NotNull(bindingList)
                .Where(x => x.Count > 1)
                .Where(x => BindingPrecedenceOrder(x[0], x[1]) != 0)
                .Select(x => x[0])
                .Or(() => fallThroughSelection(request, bindingList))
                .ValueOrDefault();
        }

        public static int BindingPrecedenceOrder(IExodataBinding x, IExodataBinding y)
        {
            var left = x as IExodataBindingDetails;
            var right = y as IExodataBindingDetails;

            if (left == null || right == null)
                return 0;

            bool leftIsAttributeBinding = left.Source is AttributeExodataBindingSource;
            bool rightIsAttributeBinding = right.Source is AttributeExodataBindingSource;

            if (leftIsAttributeBinding ^ rightIsAttributeBinding)
                return leftIsAttributeBinding ? 1 : -1;

            bool leftBoundToContext = left.BoundToContextInstance;
            bool rightBoundToContext = right.BoundToContextInstance;

            if (leftBoundToContext ^ rightBoundToContext)
                return leftBoundToContext ? -1 : 1;

            bool leftBoundToSubject = left.BoundToSubjectInstance;
            bool rightBoundToSubject = right.BoundToSubjectInstance;

            if (leftBoundToSubject ^ rightBoundToSubject)
                return leftBoundToSubject ? -1 : 1;

            if (left.ContextType != right.ContextType)
                return left.ContextType.IsAssignableFrom(right.ContextType) ? 1 : -1;

            if (left.SubjectType != right.SubjectType)
                return left.SubjectType.IsAssignableFrom(right.SubjectType) ? 1 : -1;

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

        public IFluentExodataBindingGivenSubjectWhenScopeTo<TExodata, object, object> Bind<TExodata>(ISymbol<TExodata> symbol)
        {
            return _ResolverModule.Bind(symbol);
        }

        public void Bind<TExodata>(ISymbol<TExodata> symbol, TExodata value)
        {
            Bind(symbol).To(value);
        }
    }
}
