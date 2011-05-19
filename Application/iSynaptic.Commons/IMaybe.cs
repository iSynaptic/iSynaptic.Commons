using System;

namespace iSynaptic.Commons
{
    public interface IMaybe<out T> : IMaybe
    {
        new T Value { get; }
    }

    public interface IMaybe
    {
        object Value { get; }
        bool HasValue { get; }
        Exception Exception { get; }
    }
}