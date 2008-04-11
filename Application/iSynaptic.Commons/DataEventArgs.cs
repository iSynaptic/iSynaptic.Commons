using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public class DataEventArgs<T> : EventArgs
    {
        public DataEventArgs(T data)
        {
            Data = data;
        }

        public T Data { get; private set; }
    }
}
