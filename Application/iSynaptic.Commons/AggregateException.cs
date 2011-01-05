using System;
using System.Collections.Generic;
using System.Linq;

namespace iSynaptic.Commons
{
    public class AggregateException : Exception
    {
        private readonly IEnumerable<Exception> _Exceptions = null;

        public AggregateException(string message, IEnumerable<Exception> exceptions) : base(message)
        {
            Guard.NotNull(exceptions, "exceptions");

            var exceptionArray = exceptions.ToArray();
            Guard.NotEmpty(exceptionArray, "exceptions");

            _Exceptions = exceptionArray;
        }

        public IEnumerable<Exception> Exceptions
        {
            get { return _Exceptions; }
        }
    }
}
