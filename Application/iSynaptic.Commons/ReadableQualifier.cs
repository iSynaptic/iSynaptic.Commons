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

        public ReadableQualifier(Func<TQualifier, TItem> getByQualifier, Func<IEnumerable<TQualifier>> knownQualifiers)
        {
            _GetByQualifier = Guard.NotNull(getByQualifier, "getByQualifier");
            _GetQualifiers = knownQualifiers ?? (() => Enumerable.Empty<TQualifier>());
        }

        public TItem this[TQualifier qualifier]
        {
            get { return _GetByQualifier(qualifier); }
        }

        public IEnumerable<TQualifier> GetQualifiers()
        {
            return _GetQualifiers();
        }
    }
}
