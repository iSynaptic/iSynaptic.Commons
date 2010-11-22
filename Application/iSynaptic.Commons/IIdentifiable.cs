using System;

namespace iSynaptic.Commons
{
    public interface IIdentifiable<T> where T : IEquatable<T>
    {
        T GetIdentifier();
    }
}
