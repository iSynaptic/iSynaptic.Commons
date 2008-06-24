using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace iSynaptic.Commons.Extensions
{
    internal class PipelinedEnumerable<T> : IPipelinedEnumerable<T>
    {
        private IEnumerable<T> _InnerEnumerable = null;
        private Func<IEnumerable<T>, IEnumerable<T>> _Processor = null;

        public PipelinedEnumerable(IEnumerable<T> innerEnumerable, Func<IEnumerable<T>, IEnumerable<T>> processor)
        {
            if (processor == null)
                throw new ArgumentNullException("processor");

            _InnerEnumerable = innerEnumerable;
            _Processor = processor;
        }

        public PipelinedEnumerable(IEnumerable<T> innerEnumerable, Func<T, T> processor)
        {
            if (processor == null)
                throw new ArgumentNullException("processor");

            _InnerEnumerable = innerEnumerable;
            _Processor = Wrap(processor);
        }

        private Func<IEnumerable<T>, IEnumerable<T>> Wrap(Func<T, T> processor)
        {
            return e =>
            {
                return WrapHelper(e, processor);
            };
        }

        private IEnumerable<T> WrapHelper(IEnumerable<T> source, Func<T, T> processor)
        {
            foreach (T item in source)
                yield return processor(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _Processor(_InnerEnumerable).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
