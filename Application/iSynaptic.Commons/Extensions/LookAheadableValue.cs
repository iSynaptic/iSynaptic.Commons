using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Extensions
{
    public class LookAheadableValue<T>
    {
        private T _Value = default(T);
        private LookAheadEnumerator<T> _Enumerator = null;

        internal LookAheadableValue(T value, LookAheadEnumerator<T> enumerator)
        {
            _Value = value;
            _Enumerator = enumerator;
        }

        public T LookAhead(int index)
        {
            return _Enumerator.LookAhead(index);
        }

        public T Value
        {
            get { return _Value; }
        }
    }
}
