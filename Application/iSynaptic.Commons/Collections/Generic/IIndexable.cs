using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public interface IIndexable : IEnumerable
    {
        object this[int index] { get; }
        int Length { get; }
    }

    public interface IIndexable<out T> : IIndexable, IEnumerable<T>
    {
        new T this[int index] { get; }
    }
}
