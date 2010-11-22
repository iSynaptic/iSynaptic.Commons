using System;

namespace iSynaptic.Commons
{
    public class DataEventArgs : EventArgs
    {
        public DataEventArgs(object data)
        {
            Data = data;
        }

        public object Data { get; private set; }
    }

    public class DataEventArgs<T> : DataEventArgs
    {
        public DataEventArgs(T data) : base(data)
        {
        }

        public new T Data
        {
            get { return (T) base.Data; }
        }
    }
}
