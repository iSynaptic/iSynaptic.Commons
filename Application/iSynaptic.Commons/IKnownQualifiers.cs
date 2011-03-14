using System.Collections.Generic;

namespace iSynaptic.Commons
{
    public interface IKnownQualifiers<out TQualifier>
    {
        IEnumerable<TQualifier> GetQualifiers();
    }
}
