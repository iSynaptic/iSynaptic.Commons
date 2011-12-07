using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Concurrent
{
    public static class ProducerConsumerCollectionExtensions
    {
        public static Maybe<T> TryTake<T>(this IProducerConsumerCollection<T> source)
        {
            Guard.NotNull(source, "source");
            return Maybe.TrySelect<T>(source.TryTake);
        }
    }
}
