using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public class ProjectionCollection<TSourceItem, TProjectedItem> : ICollection<TProjectedItem>
    {
        private static NotSupportedException _ReadOnlyException = new NotSupportedException("Mutating a readonly collection is not allowed.");

        private readonly ICollection<TSourceItem> _Underlying = null;
        private readonly Func<TSourceItem, TProjectedItem> _Selector = null;

        public ProjectionCollection(ICollection<TSourceItem> underlying, Func<TSourceItem, TProjectedItem> selector)
        {
            Guard.NotNull(underlying, "underlying");
            Guard.NotNull(selector, "selector");

            _Underlying = underlying;
            _Selector = selector;
        }

        public IEnumerator<TProjectedItem> GetEnumerator()
        {
            return _Underlying.Select(_Selector).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<TProjectedItem>.Add(TProjectedItem item)
        {
            throw _ReadOnlyException;
        }

        void ICollection<TProjectedItem>.Clear()
        {
            throw _ReadOnlyException;
        }

        public bool Contains(TProjectedItem item)
        {
            var predicate = item == null ? (Func<TSourceItem, bool>) (x => _Selector(x) == null) : (x => item.Equals(_Selector(x)));
            return _Underlying.Any(predicate);
        }

        public void CopyTo(TProjectedItem[] destination, int index)
        {
            _Underlying
                .Select(_Selector)
                .CopyTo(destination, index);
        }

        bool ICollection<TProjectedItem>.Remove(TProjectedItem item)
        {
            throw _ReadOnlyException;
        }

        public int Count
        {
            get { return _Underlying.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }
    }
}
