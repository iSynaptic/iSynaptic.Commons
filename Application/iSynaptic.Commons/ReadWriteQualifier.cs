using System;
using System.Collections.Generic;
using System.Linq;

namespace iSynaptic.Commons
{
    public class ReadWriteQualifier<TQualifier, TItem> : ReadableQualifier<TQualifier, TItem>, IReadWriteQualifier<TQualifier, TItem>
    {
        private readonly Action<TQualifier, TItem> _SetByQualifier = null;

        public ReadWriteQualifier(Func<TQualifier, TItem> getByQualifier, Action<TQualifier, TItem> setByQualifier)
            : this(getByQualifier, setByQualifier, null)
        {
        }

        public ReadWriteQualifier(Func<TQualifier, TItem> getByQualifier, Action<TQualifier, TItem> setByQualifier, Func<IEnumerable<TQualifier>> knownQualifiers)
            : base(getByQualifier, knownQualifiers)
        {
           _SetByQualifier = Guard.NotNull(setByQualifier, "setByQualifier");
        }

        TItem IReadWriteQualifier<TQualifier, TItem>.this[TQualifier qualifier]
        {
            get { return base[qualifier]; }
            set { _SetByQualifier(qualifier, value); }
        }
    }
}