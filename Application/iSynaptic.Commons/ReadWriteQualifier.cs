using System;
using System.Collections.Generic;
using System.Linq;

namespace iSynaptic.Commons
{
    public class ReadWriteQualifier<TQualifier, TItem> : IReadWriteQualifier<TQualifier, TItem>, IKnownQualifiers<TQualifier>
    {
        private readonly Func<TQualifier, TItem> _GetByQualifier = null;
        private readonly Action<TQualifier, TItem> _SetByQualifier = null;
        private readonly Func<IEnumerable<TQualifier>> _GetQualifiers = null;

        public ReadWriteQualifier(Func<TQualifier, TItem> getByQualifier, Action<TQualifier, TItem> setByQualifier)
            : this(getByQualifier, setByQualifier, null)
        {
        }

        public ReadWriteQualifier(Func<TQualifier, TItem> getByQualifier, Action<TQualifier, TItem> setByQualifier, Func<IEnumerable<TQualifier>> knownQualifier)
        {
            Guard.NotNull(getByQualifier, "getByQualifier");
            Guard.NotNull(setByQualifier, "setByQualifier");

            _GetByQualifier = getByQualifier;
            _SetByQualifier = setByQualifier;
            _GetQualifiers = knownQualifier ?? (() => Enumerable.Empty<TQualifier>());
        }

        TItem IReadWriteQualifier<TQualifier, TItem>.this[TQualifier qualifier]
        {
            get { return _GetByQualifier(qualifier); }
            set { _SetByQualifier(qualifier, value); }
        }

        IEnumerable<TQualifier> IKnownQualifiers<TQualifier>.GetQualifiers()
        {
            return _GetQualifiers();
        }
    }
}