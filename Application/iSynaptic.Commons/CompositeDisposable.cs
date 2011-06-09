using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public class CompositeDisposable : ICollection<IDisposable>, IDisposable
    {
        private readonly List<IDisposable> _Disposables = new List<IDisposable>();

        public T Add<T>(T disposable) where T : IDisposable
        {
            Guard.NotNull(disposable, "disposable");

            _Disposables.Add(disposable);
            return disposable;
        }

        public void Add(IDisposable item)
        {
            _Disposables.Add(item);
        }

        public bool Contains(IDisposable disposable)
        {
            Guard.NotNull(disposable, "disposable");
            return _Disposables.Contains(disposable);
        }

        public void CopyTo(IDisposable[] array, int arrayIndex)
        {
            _Disposables.CopyTo(array, arrayIndex);
        }

        public bool Remove(IDisposable item)
        {
            return _Disposables.Remove(item);
        }

        public int Count
        {
            get { return _Disposables.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Clear()
        {
            _Disposables.Clear();
        }

        public IEnumerator<IDisposable> GetEnumerator()
        {
            return _Disposables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            var exceptions = new List<Exception>();

            Action<IDisposable> dispose = d => d.Dispose();
            dispose = dispose.CatchExceptions(exceptions);

            _Disposables
                .ToList()
                .ForEach(dispose);

            Clear();

            if (exceptions.Count > 0)
                throw new AggregateException("Exception(s) occured during disposal.", exceptions);
        }

    }
}
