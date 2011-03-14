using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public class ReadableQualifier<TQualifier, TItem> : IReadableQualifier<TQualifier, TItem>, IKnownQualifiers<TQualifier>
    {
        private readonly Func<TQualifier, TItem> _GetByQualifier = null;
        private readonly Func<IEnumerable<TQualifier>> _GetQualifiers = null;

        public ReadableQualifier(Func<TQualifier, TItem> getByQualifier) : this(getByQualifier, null)
        {
        }

        public ReadableQualifier(Func<TQualifier, TItem> getByQualifier, Func<IEnumerable<TQualifier>> knownQualifier)
        {
            Guard.NotNull(getByQualifier, "getByQualifier");
            _GetByQualifier = getByQualifier;
            _GetQualifiers = knownQualifier ?? (() => Enumerable.Empty<TQualifier>());
        }

        TItem IReadableQualifier<TQualifier, TItem>.this[TQualifier qualifier]
        {
            get { return _GetByQualifier(qualifier); }
        }

        IEnumerable<TQualifier> IKnownQualifiers<TQualifier>.GetQualifiers()
        {
            return _GetQualifiers();
        }
    }
}
