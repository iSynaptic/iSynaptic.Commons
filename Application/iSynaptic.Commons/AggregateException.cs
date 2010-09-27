using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace iSynaptic.Commons
{
    public class AggregateException : Exception
    {
        private IEnumerable<Exception> _Exceptions = null;

        public AggregateException(string message, IEnumerable<Exception> exceptions) : base(message)
        {
            if(exceptions == null)
                throw new ArgumentNullException("exceptions");

            var exceptionArray = exceptions.ToArray();

            if(exceptionArray.Length <= 0)
                throw new ArgumentException("You must provide at least one exception.", "exceptions");

            _Exceptions = exceptionArray;
        }

        public IEnumerable<Exception> Exceptions
        {
            get { return _Exceptions; }
        }
    }
}
