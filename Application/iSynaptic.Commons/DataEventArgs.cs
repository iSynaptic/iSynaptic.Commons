using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public class DataEventArgs<T> : EventArgs
    {
        private T _Data = default(T);

        public DataEventArgs(T data)
        {
            _Data = data;
        }

        public T Data
        {
            get { return _Data; }
        }
    }
}
