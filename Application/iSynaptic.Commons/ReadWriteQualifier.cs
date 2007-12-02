using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public partial class ReadWriteQualifier<Q, T> : ReadableQualifier<Q, T>
    {
        private Action<Q, T> _SetHandler = null;

        public ReadWriteQualifier(Func<Q, T> getHandler, Action<Q, T> setHandler)
            : this(getHandler, setHandler, null)
        {
        }

        public ReadWriteQualifier(Func<Q, T> getHandler, Action<Q, T> setHandler, Func<Q[]> getKnownQualifiersHandler) : base(getHandler, getKnownQualifiersHandler)
        {
            if (setHandler == null)
                throw new ArgumentNullException("setHandler");

            _SetHandler = setHandler;
        }

        public new T this[Q qualifier]
        {
            get { return base[qualifier]; }
            set { _SetHandler(qualifier, value); }
        }
    }
}
