using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public interface IIdentifiable<T> where T : IEquatable<T>
    {
        T GetIdentifier();
    }
}
