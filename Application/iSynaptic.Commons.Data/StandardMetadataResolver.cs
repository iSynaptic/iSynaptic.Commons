using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace iSynaptic.Commons.Data
{
    public class StandardMetadataResolver : MetadataResolver
    {
        private readonly List<MetadataBindingModule> _Modules = new List<MetadataBindingModule>();

        private class ModuleMetadataBindingSource : IMetadataBindingSource
        {
            private readonly StandardMetadataResolver _Parent;

            public ModuleMetadataBindingSource(StandardMetadataResolver parent)
            {
                Guard.NotNull(parent, "parent");
                _Parent = parent;
            }

            public IEnumerable<IMetadataBinding<TMetadata, TSubject>> GetBindingsFor<TMetadata, TSubject>(IMetadataRequest<TMetadata, TSubject> request)
            {
                return _Parent._Modules
                    .SelectMany(x => x.GetBindingsFor<TMetadata, TSubject>(request));
            }
        }

        public StandardMetadataResolver(params MetadataBindingModule[] modules)
        {
            if (modules != null && modules.Length > 0)
            {
                foreach (var module in modules)
                    LoadModule(module);
            }

            AddMetadataBindingSource(new ModuleMetadataBindingSource(this));
            AddMetadataBindingSource<MetadataSurrogateBindingSource>();
            AddMetadataBindingSource<AttributeMetadataBindingSource>();
        }

        protected override IMetadataBinding<TMetadata, TSubject> SelectBinding<TMetadata, TSubject>(IMetadataRequest<TMetadata, TSubject> request, IEnumerable<IMetadataBinding<TMetadata, TSubject>> candidates)
        {
            var bindingList = candidates
                .ToList();

            if (bindingList.Count > 1)
            {
                bindingList.Sort(BindingSortPriority);
                if (BindingSortPriority(bindingList[0], bindingList[1]) != 0)
                    return bindingList[0];
            }

            return base.SelectBinding(request, bindingList);
        }

        private static int BindingSortPriority<TMetadata, TSubject>(IMetadataBinding<TMetadata, TSubject> left, IMetadataBinding<TMetadata, TSubject> right)
        {
            if (!(left.Source is AttributeMetadataBindingSource) && right.Source is AttributeMetadataBindingSource)
                return -1;

            if (!(right.Source is AttributeMetadataBindingSource) && left.Source is AttributeMetadataBindingSource)
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

        public void LoadModule(MetadataBindingModule module)
        {
            Guard.NotNull(module, "module");
            _Modules.Add(module);
        }

        public void UnloadModule(MetadataBindingModule module)
        {
            Guard.NotNull(module, "module");
            _Modules.Remove(module);
        }
    }
}
