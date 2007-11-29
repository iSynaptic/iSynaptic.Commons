using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public partial class ReadWriteQualifier<Q, T>
    {
        private Func<Q, T> _GetHandler = null;
        private Action<Q, T> _SetHandler = null;
        private Func<Q[]> _GetKnownQualifiersHandler = null;

        public ReadWriteQualifier(Func<Q, T> getHandler, Action<Q, T> setHandler)
            : this(getHandler, setHandler, null)
        {
        }

        public ReadWriteQualifier(Func<Q, T> getHandler, Action<Q, T> setHandler, Func<Q[]> getKnownQualifiersHandler)
        {
            if (getHandler == null)
                throw new ArgumentNullException("getHandler");

            if (setHandler == null)
                throw new ArgumentNullException("setHandler");

            _GetHandler = getHandler;
            _SetHandler = setHandler;
            _GetKnownQualifiersHandler = getKnownQualifiersHandler;
        }

        public T this[Q qualifier]
        {
            get { return _GetHandler(qualifier); }
            set { _SetHandler(qualifier, value); }
        }

        public Q[] KnownQualifiers
        {
            get
            {
                if (_GetKnownQualifiersHandler != null)
                    return _GetKnownQualifiersHandler();
                else
                    return null;
            }
        }
    }
}
