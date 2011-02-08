using System;

namespace iSynaptic.Commons
{
    public interface IMaybe<out T>
    {
        T Value { get; }
        bool HasValue { get; }
        Exception Exception { get; }
    }
}