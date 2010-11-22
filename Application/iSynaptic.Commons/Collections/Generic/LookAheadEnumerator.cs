﻿using System;
using System.Collections.Generic;
using System.Collections;

namespace iSynaptic.Commons.Collections.Generic
{
    internal class LookAheadEnumerator<T> : IEnumerator<LookAheadableValue<T>>
    {
        private readonly IEnumerator<T> _InnerEnumerator = null;
        private List<T> _LookAheadList = null;

        private T _Current = default(T);
        private bool _Disposed = false;

        public LookAheadEnumerator(IEnumerator<T> innerEnumerator)
        {
            _InnerEnumerator = innerEnumerator;
        }

        public void Dispose()
        {
            _InnerEnumerator.Dispose();
            _Disposed = true;
        }

        public bool MoveNext()
        {
            if (_Disposed)
                throw new ObjectDisposedException("IEnumerator<T>");

            if (LookAheadList.Count <= 0)
            {
                bool results = _InnerEnumerator.MoveNext();

                _Current = _InnerEnumerator.Current;
                return results;
            }

            _Current = LookAheadList[0];
            LookAheadList.RemoveAt(0);

            return true;
        }

        public void Reset()
        {
            if (_Disposed)
                throw new ObjectDisposedException("IEnumerator<T>");

            _InnerEnumerator.Reset();
            _LookAheadList = null;
        }

        public T LookAhead(int index)
        {
            if (_Disposed)
                throw new ObjectDisposedException("IEnumerator<T>");

            if (LookAheadList.Count >= (index + 1))
                return LookAheadList[index];

            int itemsToEnumerate = (index + 1) - LookAheadList.Count;

            for (int itemsEnumerated = 0; itemsEnumerated < itemsToEnumerate; itemsEnumerated++)
            {
                bool results = _InnerEnumerator.MoveNext();
                if (results)
                {
                    LookAheadList.Add(_InnerEnumerator.Current);
                }
                else
                    throw new IndexOutOfRangeException();
            }

            return LookAheadList[index];
        }

        public LookAheadableValue<T> Current
        {
            get { return new LookAheadableValue<T>(_Current, this); }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        protected List<T> LookAheadList
        {
            get { return _LookAheadList ?? (_LookAheadList = new List<T>()); }
        }
    }
}
