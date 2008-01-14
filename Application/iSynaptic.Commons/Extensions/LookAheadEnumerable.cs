using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Extensions
{
    internal class LookAheadEnumerable<T> : IEnumerable<LookAheadableValue<T>>
    {
        private IEnumerable<T> _InnerEnumerable = null;

        public LookAheadEnumerable(IEnumerable<T> innerEnumerable)
        {
            _InnerEnumerable = innerEnumerable;
        }

        public IEnumerator<LookAheadableValue<T>> GetEnumerator()
        {
            IEnumerator<T> innerEnumerator = _InnerEnumerable.GetEnumerator();
            if (innerEnumerator == null)
                return null;

            return new LookAheadEnumerator<T>(innerEnumerator);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
