using System.Collections.Generic;

namespace iSynaptic.Commons
{
    public interface IKnownQualifiers<T>
    {
        IEnumerable<T> GetQualifiers();
    }
}
