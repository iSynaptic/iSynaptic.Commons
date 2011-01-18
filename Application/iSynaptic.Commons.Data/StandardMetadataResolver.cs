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

            public IEnumerable<IMetadataBinding<TMetadata>> GetBindingsFor<TMetadata>(MetadataRequest<TMetadata> request)
            {
                return _Parent._Modules
                    .SelectMany(x => x.GetBindingsFor(request));
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

        protected override IMetadataBinding<TMetadata> SelectBinding<TMetadata>(MetadataRequest<TMetadata> request, IEnumerable<IMetadataBinding<TMetadata>> candidates)
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

        private static int BindingSortPriority<TMetadata>(IMetadataBinding<TMetadata> left, IMetadataBinding<TMetadata> right)
        {
            if (!(left.Source is AttributeMetadataBindingSource) && right.Source is AttributeMetadataBindingSource)
                return -1;

            if (!(right.Source is AttributeMetadataBindingSource) && left.Source is AttributeMetadataBindingSource)
                return 1;

            if (left.Member != null && right.Member == null)
                return -1;

            if (right.Member != null && left.Member == null)
                return 1;

            if (left.Subject != null && right.Subject == null)
                return -1;

            if (right.Subject != null && left.Subject == null)
                return 1;

            Type leftType = left.Subject as Type;
            Type rightType = right.Subject as Type;

            if (leftType != null && rightType == null)
                return 1;

            if (rightType != null && leftType == null)
                return -1;

            if(leftType != null && leftType != rightType)
            {
                if (leftType.IsAssignableFrom(rightType))
                    return 1;

                if (rightType.IsAssignableFrom(leftType))
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
