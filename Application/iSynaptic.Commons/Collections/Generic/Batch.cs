using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public class Batch<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _Batch = null;

        public Batch(int index, int size, IEnumerable<T> batch)
        {
            Index = Guard.MustBeGreaterThanOrEqual(index, 0, "index");
            Size = Guard.MustBeGreaterThan(size, 0, "size");
            _Batch = Guard.NotNull(batch, "batch");
        }

        public int Index { get; private set; }
        public int Size { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            return _Batch.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
